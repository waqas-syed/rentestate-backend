- Install Visual Studio. Currently used version is 2017.

- MySql
	- Download MySql Installer from here https://dev.mysql.com/downloads/installer/ and install the following tools:
		- MySql Community Server
		- MySql .NET Connector
		- MySql Workbench (Optional - for convenience)
	- A new MySql user needs to be created in order to make the application connect to the MySql database. The credentials of the user can be requested from the project lead. The permissions allowed to this user are:
		- CREATE
		- ALTER
		- DROP
		- SELECT
		- INSERT
		- UPDATE
		- DELETE
		- REFERENCES
		
- Google Cloud Storage - For uploading Photos
	- We have created Service account credentials on Google Cloud Console and downloaded a json file. Get this file from the project lead and place it somewhere on your computer.
	- Create a new environment variable called GOOGLE_APPLICATION_CREDENTIALS and point it's vaue to the json file.