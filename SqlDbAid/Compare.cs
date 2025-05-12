using System;
using System.Collections;

namespace SqlDbAid
{
    #region CompareEngine

    public enum DiffEngineLevel
    {
        FastImperfect,
        Medium,
        SlowPerfect
    }

    public class DiffEngine
    {
        private IDiffList mSource;
        private IDiffList mDest;
        private ArrayList mMatchList;

        private DiffEngineLevel mLevel;

        private DiffStateList mStateList;

        public DiffEngine()
        {
            mSource = null;
            mDest = null;
            mMatchList = null;
            mStateList = null;
            mLevel = DiffEngineLevel.FastImperfect;
        }

        private int GetSourceMatchLength(int destIndex, int sourceIndex, int maxLength)
        {
            int matchCount;
            for (matchCount = 0; matchCount < maxLength; matchCount++)
            {
                if (mDest.GetByIndex(destIndex + matchCount).CompareTo(mSource.GetByIndex(sourceIndex + matchCount)) != 0)
                {
                    break;
                }
            }
            return matchCount;
        }

        private void GetLongestSourceMatch(DiffState curItem, int destIndex, int destEnd, int sourceStart, int sourceEnd)
        {

            int maxDestLength = (destEnd - destIndex) + 1;
            int curLength = 0;
            int curBestLength = 0;
            int curBestIndex = -1;
            int maxLength = 0;
            for (int sourceIndex = sourceStart; sourceIndex <= sourceEnd; sourceIndex++)
            {
                maxLength = Math.Min(maxDestLength, (sourceEnd - sourceIndex) + 1);
                if (maxLength <= curBestLength)
                {
                    //No chance to find a longer one any more
                    break;
                }
                curLength = GetSourceMatchLength(destIndex, sourceIndex, maxLength);
                if (curLength > curBestLength)
                {
                    //This is the best match so far
                    curBestIndex = sourceIndex;
                    curBestLength = curLength;
                }
                //jump over the match
                sourceIndex += curBestLength;
            }

            if (curBestIndex == -1)
            {
                curItem.SetNoMatch();
            }
            else
            {
                curItem.SetMatch(curBestIndex, curBestLength);
            }

        }

        private void ProcessRange(int destStart, int destEnd, int sourceStart, int sourceEnd)
        {
            int curBestIndex = -1;
            int curBestLength = -1;
            int maxPossibleDestLength = 0;
            DiffState curItem = null;
            DiffState bestItem = null;
            for (int destIndex = destStart; destIndex <= destEnd; destIndex++)
            {
                maxPossibleDestLength = (destEnd - destIndex) + 1;
                if (maxPossibleDestLength <= curBestLength)
                {
                    //we won't find a longer one even if we looked
                    break;
                }
                curItem = mStateList.GetByIndex(destIndex);

                if (!curItem.HasValidLength(sourceStart, sourceEnd, maxPossibleDestLength))
                {
                    //recalc new best length since it isn't valid or has never been done.
                    GetLongestSourceMatch(curItem, destIndex, destEnd, sourceStart, sourceEnd);
                }
                if (curItem.Status == DiffStatus.Matched)
                {
                    switch (mLevel)
                    {
                        case DiffEngineLevel.FastImperfect:
                            if (curItem.Length > curBestLength)
                            {
                                //this is longest match so far
                                curBestIndex = destIndex;
                                curBestLength = curItem.Length;
                                bestItem = curItem;
                            }
                            //Jump over the match 
                            destIndex += curItem.Length - 1;
                            break;
                        case DiffEngineLevel.Medium:
                            if (curItem.Length > curBestLength)
                            {
                                //this is longest match so far
                                curBestIndex = destIndex;
                                curBestLength = curItem.Length;
                                bestItem = curItem;
                                //Jump over the match 
                                destIndex += curItem.Length - 1;
                            }
                            break;
                        default:
                            if (curItem.Length > curBestLength)
                            {
                                //this is longest match so far
                                curBestIndex = destIndex;
                                curBestLength = curItem.Length;
                                bestItem = curItem;
                            }
                            break;
                    }
                }
            }
            if (curBestIndex < 0)
            {
                //we are done - there are no matches in this span
            }
            else
            {

                int sourceIndex = bestItem.StartIndex;
                mMatchList.Add(DiffResultSpan.CreateNoChange(curBestIndex, sourceIndex, curBestLength));
                if (destStart < curBestIndex)
                {
                    //Still have more lower destination data
                    if (sourceStart < sourceIndex)
                    {
                        //Still have more lower source data
                        // Recursive call to process lower indexes
                        ProcessRange(destStart, curBestIndex - 1, sourceStart, sourceIndex - 1);
                    }
                }
                int upperDestStart = curBestIndex + curBestLength;
                int upperSourceStart = sourceIndex + curBestLength;
                if (destEnd > upperDestStart)
                {
                    //we still have more upper dest data
                    if (sourceEnd > upperSourceStart)
                    {
                        //set still have more upper source data
                        // Recursive call to process upper indexes
                        ProcessRange(upperDestStart, destEnd, upperSourceStart, sourceEnd);
                    }
                }
            }
        }

        public double ProcessDiff(IDiffList source, IDiffList destination, DiffEngineLevel level)
        {
            mLevel = level;
            return ProcessDiff(source, destination);
        }

        public double ProcessDiff(IDiffList source, IDiffList destination)
        {
            DateTime dt = DateTime.Now;
            mSource = source;
            mDest = destination;
            mMatchList = new ArrayList();

            int dcount = mDest.Count();
            int scount = mSource.Count();


            if ((dcount > 0) && (scount > 0))
            {
                mStateList = new DiffStateList(dcount);
                ProcessRange(0, dcount - 1, 0, scount - 1);
            }

            TimeSpan ts = DateTime.Now - dt;
            return ts.TotalSeconds;
        }

        private bool AddChanges(
            ArrayList report,
            int curDest,
            int nextDest,
            int curSource,
            int nextSource)
        {
            bool retval = false;
            int diffDest = nextDest - curDest;
            int diffSource = nextSource - curSource;
            int minDiff = 0;
            if (diffDest > 0)
            {
                if (diffSource > 0)
                {
                    minDiff = Math.Min(diffDest, diffSource);
                    report.Add(DiffResultSpan.CreateReplace(curDest, curSource, minDiff));
                    if (diffDest > diffSource)
                    {
                        curDest += minDiff;
                        report.Add(DiffResultSpan.CreateAddDestination(curDest, diffDest - diffSource));
                    }
                    else
                    {
                        if (diffSource > diffDest)
                        {
                            curSource += minDiff;
                            report.Add(DiffResultSpan.CreateDeleteSource(curSource, diffSource - diffDest));
                        }
                    }
                }
                else
                {
                    report.Add(DiffResultSpan.CreateAddDestination(curDest, diffDest));
                }
                retval = true;
            }
            else
            {
                if (diffSource > 0)
                {
                    report.Add(DiffResultSpan.CreateDeleteSource(curSource, diffSource));
                    retval = true;
                }
            }
            return retval;
        }

        public ArrayList DiffReport()
        {
            ArrayList retval = new ArrayList();
            int dcount = mDest.Count();
            int scount = mSource.Count();

            //Deal with the special case of empty files
            if (dcount == 0)
            {
                if (scount > 0)
                {
                    retval.Add(DiffResultSpan.CreateDeleteSource(0, scount));
                }
                return retval;
            }
            else
            {
                if (scount == 0)
                {
                    retval.Add(DiffResultSpan.CreateAddDestination(0, dcount));
                    return retval;
                }
            }


            mMatchList.Sort();
            int curDest = 0;
            int curSource = 0;
            DiffResultSpan last = null;

            //Process each match record
            foreach (DiffResultSpan drs in mMatchList)
            {
                if ((!AddChanges(retval, curDest, drs.DestIndex, curSource, drs.SourceIndex)) &&
                    (last != null))
                {
                    last.AddLength(drs.Length);
                }
                else
                {
                    retval.Add(drs);
                }
                curDest = drs.DestIndex + drs.Length;
                curSource = drs.SourceIndex + drs.Length;
                last = drs;
            }

            //Process any tail end data
            AddChanges(retval, curDest, dcount, curSource, scount);

            return retval;
        }
    }

    #endregion

    #region Structures

    public interface IDiffList
    {
        int Count();
        IComparable GetByIndex(int index);
    }

    internal enum DiffStatus
    {
        Matched = 1,
        NoMatch = -1,
        Unknown = -2

    }

    internal class DiffState
    {
        private const int BAD_INDEX = -1;
        private int mStartIndex;
        private int mLength;

        public int StartIndex { get { return mStartIndex; } }
        public int EndIndex { get { return ((mStartIndex + mLength) - 1); } }
        public int Length
        {
            get
            {
                int len;
                if (mLength > 0)
                {
                    len = mLength;
                }
                else
                {
                    if (mLength == 0)
                    {
                        len = 1;
                    }
                    else
                    {
                        len = 0;
                    }
                }
                return len;
            }
        }

        public DiffStatus Status
        {
            get
            {
                DiffStatus stat;
                if (mLength > 0)
                {
                    stat = DiffStatus.Matched;
                }
                else
                {
                    switch (mLength)
                    {
                        case -1:
                            stat = DiffStatus.NoMatch;
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(mLength == -2, "Invalid status: mLength < -2");
                            stat = DiffStatus.Unknown;
                            break;
                    }
                }
                return stat;
            }
        }

        public DiffState()
        {
            SetToUnkown();
        }

        protected void SetToUnkown()
        {
            mStartIndex = BAD_INDEX;
            mLength = (int)DiffStatus.Unknown;
        }

        public void SetMatch(int start, int length)
        {
            System.Diagnostics.Debug.Assert(length > 0, "Length must be greater than zero");
            System.Diagnostics.Debug.Assert(start >= 0, "Start must be greater than or equal to zero");
            mStartIndex = start;
            mLength = length;
        }

        public void SetNoMatch()
        {
            mStartIndex = BAD_INDEX;
            mLength = (int)DiffStatus.NoMatch;
        }


        public bool HasValidLength(int newStart, int newEnd, int maxPossibleDestLength)
        {
            if (mLength > 0) //have unlocked match
            {
                if ((maxPossibleDestLength < mLength) ||
                    ((mStartIndex < newStart) || (EndIndex > newEnd)))
                {
                    SetToUnkown();
                }
            }
            return (mLength != (int)DiffStatus.Unknown);
        }
    }

    internal class DiffStateList
    {
        private DiffState[] mArray;

        public DiffStateList(int destCount)
        {
            mArray = new DiffState[destCount];
        }

        public DiffState GetByIndex(int index)
        {
            DiffState retval = mArray[index];
            if (retval == null)
            {
                retval = new DiffState();
                mArray[index] = retval;
            }

            return retval;
        }
    }

    public enum DiffResultSpanStatus
    {
        NoChange,
        Changed,
        DestinationMissing,
        SourceMissing
    }

    public class DiffResultSpan : IComparable
    {
        private const int BAD_INDEX = -1;
        private int mDestIndex;
        private int mSourceIndex;
        private int mLength;
        private DiffResultSpanStatus mStatus;

        public int DestIndex { get { return mDestIndex; } }
        public int SourceIndex { get { return mSourceIndex; } }
        public int Length { get { return mLength; } }
        public DiffResultSpanStatus Status { get { return mStatus; } }

        protected DiffResultSpan(
            DiffResultSpanStatus status,
            int destIndex,
            int sourceIndex,
            int length)
        {
            mStatus = status;
            mDestIndex = destIndex;
            mSourceIndex = sourceIndex;
            mLength = length;
        }

        public static DiffResultSpan CreateNoChange(int destIndex, int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.NoChange, destIndex, sourceIndex, length);
        }

        public static DiffResultSpan CreateReplace(int destIndex, int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.Changed, destIndex, sourceIndex, length);
        }

        public static DiffResultSpan CreateDeleteSource(int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.DestinationMissing, BAD_INDEX, sourceIndex, length);
        }

        public static DiffResultSpan CreateAddDestination(int destIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.SourceMissing, destIndex, BAD_INDEX, length);
        }

        public void AddLength(int i)
        {
            mLength += i;
        }

        public override string ToString()
        {
            return string.Format("{0} (Dest: {1},Source: {2}) {3}",
                mStatus.ToString(),
                mDestIndex.ToString(),
                mSourceIndex.ToString(),
                mLength.ToString());
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return mDestIndex.CompareTo(((DiffResultSpan)obj).mDestIndex);
        }

        #endregion
    }

    #endregion

    #region TextFile

    public class TextLine : IComparable
    {
        public string mLine;
        private int mHash;

        public string Line
        {
            get
            {
                return mLine;
            }
            set
            {
                mLine = value;
            }
        }

        public TextLine(string str, bool CaseSensitive)
        {
            mLine = str.Replace("\t", "    ");
            if (CaseSensitive)
            {
                mHash = str.GetHashCode();
            }
            else
            {
                mHash = str.ToLower().GetHashCode();
            }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return mHash.CompareTo(((TextLine)obj).mHash);
        }

        #endregion
    }

    public class TextDiffList : IDiffList
    {
        private const int MaxLineLength = 1024;
        private ArrayList mLines;

        public TextDiffList(string Text, bool CaseSensitive)
        {
            mLines = new ArrayList();

            string[] linesArray = Text.Split(new char[] { '\n' });
            foreach (string s in linesArray)
            {
                mLines.Add(new TextLine(s, CaseSensitive));
            }
        }

        #region IDiffList Members

        public int Count()
        {
            return mLines.Count;
        }

        public IComparable GetByIndex(int index)
        {
            return (TextLine)mLines[index];
        }

        #endregion
    }

    #endregion
}
