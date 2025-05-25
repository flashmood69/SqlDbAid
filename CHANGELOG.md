# SqlDbAid Changelog

## [v2.6.7.0]
- Clear database dropdown when switching authentication modes
- Add alternating row colors to all DataGridView controls for better readability
- Remove unused Win32 API calls from CodeForm

## [v2.6.6.0]
- Microsoft Entra Password Authentication (experimental).

## [v2.6.5.0]
- Revisited SQL Highlighter plus updeted keywords.

## [v2.6.4.0]
- Fixed collation handling in the "Processes" Report and "Table Satistics" Report.
- Disabled "Deadlocks" Report on Azure SQL.

## [v2.6.3.0]
- First open source release.
- .NET Framework upgraded to 4.8.
- Added more keywords to syntax highlighting.

## [v2.6.1.7]
- Fixed an error in the "Indexes Status Report" when run from the table context menu.

## [v2.6.1.6]
- Improved "Indexes Status Report" performance.

## [v2.6.1.5]
- Improved "Indexes Status Report" performance.

## [v2.6.1.4]
- Fixed column store index scripting.
- Added "Missing Indexes" for views.

## [v2.6.1.2]
- Fixed an issue where the "Tables MBytes" report was not working correctly on large tables.

## [v2.6.1.1]
- Fixed the "Check for Update" functionality.

## [v2.6.1.0]
- The "Processes" report is no longer filtered by the current database, and its column layout has been updated.
- SqlDbAid now requires .NET Framework 4.5 or later.

## [v2.6.0.0]
- The Options form now allows managing your server list.
- The "Search Text" feature now supports regular expressions.
- Added "Deadlocks" report.
- Filtered indexes are now supported.
- Improved the "Processes" report.

## [v2.5.0.0]
- The "Indexes Status Report" now shows total reads and last read date.
- Offline Compare now highlights the most recent modify date.

## [v2.4.5.0]
- The "Processes Report" now shows the script fragment currently running (the full script has been moved into the `sql_task` column).
- Minor bugs fixed.

## [v2.4.3.0]
- Added "Column Definition Inconsistency Check" (based on a blog post by the author).

## [v2.4.2.0]
- Color scheme updated to match SQL Server 2012.
- Fixed an issue where the "Server Properties Report" was not working on SQL Server 2012.

## [v2.4.1.0]
- Added "Server Roles Report".
- Added "Database Roles Report".
- The "Jobs Report" now requires fewer privileges.

## [v2.4.0.0]
- Added "Server Configurations Report".
- Added "Server Properties Report".
- Added wait resource information to the "Processes Report".
- Minor bugs fixed.

## [v2.3.2.0]
- Added next run information to the "Jobs Report".
- Removed plan information from the "Processes Report".

## [v2.3.1.0]
- Improved "Jobs Report" speed.
- Improved "Processes Report" speed.

## [v2.3.0.0]
- Added "Database Permissions" report (based on a blog post by the author).
- Minor bugs fixed.

## [v2.2.2.3]
- Fixed date/time type formatting in the Data Export tool.

## [v2.2.2.2]
- Fixed current duration formatting in the "Jobs Report".

## [v2.2.2.1]
- Added total elapsed time to the "Processes Report".
- Added current duration to the "Jobs Report".

## [v2.2.2.0]
- Fixed Offline Compare false mismatches.
- Added "CPU Report".
- Improved the "Processes Report".

## [v2.2.0.0]
- Fixed missing first column when copying compare results.
- Fixed a possible empty login name in the "Processes Report".
- Added a query plan column to the "Processes Report".
- Added a file ID column to the "Database Files Report".
- Added "Jobs Report".

## [v2.1.0.0]
- Added "Table Statistics Report".
- Added "Top Queries (by time)" report.
- Improved the "Processes Report".
- The "Database Files Report" now includes free MBytes.
- Indexes reports now include compression information.
- The "Table MBytes Report" now requires fewer privileges.
- Offline Compare ignores carriage returns (CR) by default.
- Fixed possible duplicates in Index Statistics.
- GUI improvements.

## [v2.0.7.0]
- Fixed "Ignore Empty Lines" in the compare module (for scripts without line endings).

## [v2.0.6.0]
- Fragmentation Analysis is now optional to speed up other statistics extraction.
- Added per-table Index Statistics via the context menu.
- Added additional information in "Missing Indexes" and "Database Files" reports.
- Fixed invalid character check in suggested filenames.

## [v2.0.2.0]
- Fixed "Ignore Empty Lines" in the compare module.

## [v2.0.1.0]
- Fixed an issue where object names containing "]" were not scripted correctly.
- Fixed an issue where a shrink script in the "Database Files Report" was created when not applicable.
- Fixed "Missing Foreign Key Indexes" search.
- Fixed compare form scrolling.

## [v2.0.0.0]
- New compare engine.
- Added "Database Files Report".
- Table scripts now include indexes.

## [v1.5.8.0]
- Added a light version of the "Indexes Status Report".
- Fixed duplicates in the "Indexes Status Report".

## [v1.5.7.4]
- Fixed locks when retrieving object metadata.
- Fixed possible missing processes in the "Processes Report".

## [v1.5.7.3]
- Added selected text execution.
- Fixed an error when selecting the same column multiple times in the query editor.

## [v1.5.7.2]
- Modified Data Exporter defaults.
- Fixed some GUI issues.

## [v1.5.7.1]
- Fixed renamed objects detection.

## [v1.5.7.0]
- Added an "Update Statistics" script to the "Table MB Report".
- Added "Data Search Query".
- Fixed 32k maximum code size in the query form.
- Fixed an issue where main form checkboxes were not visible (Windows 7 only).

## [v1.5.6.0]
- Added Unicode option to the Data Exporter.
- Fixed an issue where generated insert scripts were not working when exporting NVARCHAR NULL values.

## [v1.5.5.0]
- Added "Plan Caches Report".
- Added "Use Local settings" option to the Data Exporter.
- Fixed an issue where the Data Exporter was not using the execution timeout from settings.
- Fixed an issue where "Find Next" (F3) was not working in the code viewer.

## [v1.5.4.0]
- Added "Find" (CTRL+F) to the code viewer.
- Implemented an alternative method to download newer versions.

## [v1.5.3.2]
- Added an option to avoid getting locked object names in the "Locks Report".
- Improved the "Table MBytes Report" by adding weight percentage.

## [v1.5.3.1]
- Fixed a minor syntax highlighting issue.
- Improved renamed objects detection.
- Unique constraint indexes are now scripted only inside the table definition.

## [v1.5.3.0]
- Added "Append and Run" option to directly compare databases.
- Added "Processes Report".
- Added "Locks Report".
- Added a code viewer to all reports that include a SQL script.
- Added SQL user types scripting.
- Added an option to filter queries within N minutes in the "Top Queries Report".
- Fixed an issue where case-sensitive collation prevented object scripting.
- Fixed connection leaks.
- Fixed an issue when searching text containing an open square bracket.
- Fixed an issue where the server list did not drop down after a server search.
- Improved database name identification in the "Top Queries Report".
- Improved the "Indexes Status Report" by adding new statistics.
- Improved the "Count Distinct Report" by adding selectivity.
- Improved the query form by adding open/save options.
- System databases are now hidden by default.
- Application queries are now hidden by default.
- Several GUI improvements.
- Server Activity reports (processes, locks, and top queries) are now filtered by the selected database.

## [v1.5.2.0]
- Fixed missing prefix N when generating insert scripts.
- Program name has changed.

## [v1.5.1.0]
- Fixed Unicode character scripting.
- Filtered out internal queries from the "Top Queries Report".

## [v1.5.0.1]
- Fixed a 30-second timeout in the "Count Distinct Report".
- Improved the "Count Distinct Report".

## [v1.5.0.0]
- Added existing indexes script generation.
- Added "Indexes Status Report".
- Added count of distinct table values.
- Added a basic query form.
- Added a basic table/view data viewer.
- Fixed trigger creation order.
- Fixed SQL Server 2008 new types export.
- Script execution timeout default is now 120 seconds.
- GUI improvements.

## [v1.4.2.0]
- Added support for schemas.
- Fixed an issue with incomplete table scripts after dropping the first column.

## [v1.4.1.0]
- Added "Top Expensive Queries Report".
- Added "Table MBytes Report".
- Missing indexes are now shown before the export.
- Fixed possible duplicates in missing indexes search.

## [v1.4.0.0]
- Added table/view data export functionality.
- Fixed an `ARITHABORT` error.

## [v1.3.4.2]
- Fixed missing indexes duplicated name issue.

## [v1.3.4.1]
- "Missing Foreign Key Indexes" search now requires fewer privileges.

## [v1.3.4.0]
- Fixed an issue when exporting more than two compare scripts.
- Improved object ordering for single file export.
- Syntax highlighting is now closer to SSMS.
- Added a button to locate SQL Server instances.

## [v1.3.3.0]
- Fixed an issue where a single quote in a table name broke the compare script.
- Fixed uncommon character handling in constraint scripts.

## [v1.3.2.0]
- Fixed non-clustered primary keys scripting.
- Added missing indexes script generation.
- Added table unique constraints scripting.
- Filtered out diagram objects.
- Script execution timeout default is now 30 seconds.
- Minor GUI improvements.

## [v1.3.1.0]
- Added template parameters in insert and update scripts.
- Case-sensitive compare is now optional.
- Options moved to a dedicated form.
- GUI improvements.

## [v1.3.0.1]
- Fixed compare script generation.

## [v1.3.0.0]
- Implemented syntax highlighting.
- Added text search inside code definition with highlighting.
- Added Select, Insert, and Update script generation.
- Several GUI improvements.

## [v1.2.1.0]
- Fixed a file presence check bug.

## [v1.2.0.0]
- First public release.
- Added database compare scripts generation.
- Added view code on grid double-click.
- Fixed some GUI issues.

## [v1.1.0.0]
- Internal release.
- Added "Missing FK Indexes" search.
- Fixed some GUI issues.

## [v1.0.0.0]
- Internal release.