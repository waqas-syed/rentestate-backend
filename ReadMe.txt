- FOR LOCAL DEVELOPMENT
	- Visual Studio Installation
		- Install Visual Studio. Currently used version is 2017.
			- Install .net core 1.0 as part of the Visual Studio installation. Otherwise the frontend project will not run.
			
	- Web.Config
		- In Rentstuff.Frontend.WebHost, go to web.config
		- Comment out the <Rewrite> tag.
		- In the AppSettings > FrontendUrl, change the url to the localhost url specified in the RentStuff.Frontend project. This should be http://localhost:11803

- IIS
	- Go to ‘Turn Windows features on or off’, check Internet Information Services.
	- It’s important to enable the following setting manually:
		>Internet Information Services 
			> World Wide Web Services 
				> Application Development Features 
					> ASP.NET 4.5

- MySql
	- Download MySql Installer from here https://dev.mysql.com/downloads/installer/ and install the following tools:
		- MySql Community Server
		- MySql .NET Connector
		- MySql Workbench (Optional - for convenience)
	- A new MySql user needs to be created apart from the root user in order to make the application connect to the MySql database. The credentials of the user can be requested from the project lead. The permissions allowed to this user are:
		- CREATE
		- ALTER
		- DROP
		- SELECT
		- INSERT
		- UPDATE
		- DELETE
		- REFERENCES
	- Create a new database named rentstuff
	- Import the database from a .sql dump and import the data using MySql WorkBench: 
		- Server > Data Import > Select the File > Choose from dumping the structure and data, or only data or only structure.
		
- FLYWAY - To setup the database up to the latest version. This is crucial in the case of Continuous Integration of TeamCity on the server.
	- Download and install Flyway: https://flywaydb.org/download/. Lets say the location where you copied the Flyway directory is this: C:\Program Files\flyway-5.1.4
	- Set the environment variable for the Flyway installation directory.
	- Copy the content of the folder src\RentStuff\Data\MySql\Migrations to the '/sql' folder of the flyway installation. E.g., C:\Program Files\flyway-5.1.4\sql
	- Run the following on an elevated command prompt:
		flyway migrate
		
- Google Cloud Storage - For uploading Photos
	- We have created Service account credentials on Google Cloud Console and downloaded a json file. Get this file from the project lead and place it somewhere on your computer.
	- Create a new environment variable called GOOGLE_APPLICATION_CREDENTIALS and point it's value to the json file.

- Facebook Login:
	- For OWIN pipeline in ASP.NET, the redirect url that we provide to Facebook is https://ourdomain.com/signin-facebook. So in our case it is https://api.zarqoon.com/signin-facebook.
	- We need to add this url to 'Valid OAuth Redirect Uri field' in the Facebook Developers' 'Facebook Login' product page: https://developers.facebook.com/apps/114619729160977/fb-login/