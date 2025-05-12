# SqlDbAid

<p align="center">
  <img src="Res/SDALogo.png" alt="SqlDbAid Logo" width="200"/>
</p>

SqlDbAid is an intuitive standalone application that enables you to easily script database items and data for SQL Server. It's a powerful tool for database developers and administrators to manage, compare and export SQL Server database objects and data.

## Features

### Portable

### User-Friendly Interface
<p align="center">
  <img src="Res/F01-MainForm.png" alt="Main Interface" width="700"/>
</p>

### Intuitive menu system
<p align="center">
  <img src="Res/F02-Menu.png" alt="Menu System" width="700"/>
</p>


### Database Object Scripting
- Script tables, views, stored procedures, functions, and other database objects
- Support for schemas
- Syntax highlighting for SQL code

### Database Comparison
<p align="center">
  <img src="Res/F03-Compare.png" alt="Database Comparison" width="700"/>
</p>

- Compare database objects between different databases
- Compare the same database objects between different points in time


### Data Export
<p align="center">
  <img src="Res/F04-Export.png" alt="Data Export" width="700"/>
</p>

- Export data to various formats (Text, Insert Script)
- Customizable export settings (delimiters, text qualifiers, etc.)
- Support for Unicode

### Database Reports
- Processes
- Deadlocks
- Database Statistics
- Database Indexes
- Database Permissions

## System Requirements

- Windows operating system
- .NET Framework 4.8
- Appropriate database permissions (VIEW SERVER STATE, VIEW DATABASE STATE for some features)

## Installation

1. Download the latest release from the [Releases](https://github.com/flashmood69/SqlDbAid/releases) page
2. Extract the ZIP file to your preferred location
3. Run SqlDbAid.exe

No installation is required as SqlDbAid is a portable application.

## Usage

### Connecting to a Database

1. Launch SqlDbAid
2. Enter your SQL Server instance name (you can manage a server list from the option menu)
3. Choose authentication method (Windows or SQL Server)
4. If using SQL Server authentication, enter your username and password
5. Select a database from the dropdown list (this automatically opens the connection)

### Scripting Database Objects

1. Connect to a database
2. Select the object types you want to script
3. Click "Refresh" to generate the SQL scripts
4. Choose specific objects or select all (Click Sel. column)
5. Click "Script" to export the SQL scripts

### Comparing Databases

1. Connect to a database
2. Select the object types you want to compare and click "Refresh"
3. Choose "Offline Compare" from the menu then click export selected objects
4. Repeat the same process for the target database
5. Choose "Offline Compare" from the menu then click compare exported objects
6. Select two files to compare then click "Compare"

Offline compare allows you to compare different point in time of the same database.

### Exporting Data

1. Connect to a database
2. Right click on a table object and select "Export Data"
3. Configure export settings (format, delimiters, etc.)
4. Click "Export" to generate the output file

### Shortcuts and Hints

1. Double click a row in the main window to view the script (CTRL+F / F3 to search)
2. Right click a row in the main window to open the context menu
3. Double click a row in the Processes report window to view the process script

## License

This is free and unencumbered software released into the public domain. See the [LICENSE](LICENSE) file for details.

## Changelog

See the [CHANGELOG.md](CHANGELOG.md) file for details on version history and updates.

## Contributing

Contributions are welcome! Feel free to submit issues or pull requests to help improve SqlDbAid.

## Support

For support, please open an issue on the [GitHub repository](https://github.com/flashmood69/SqlDbAid/issues).