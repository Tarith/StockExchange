1- Steps to create and initialize the database

To create and inizialize the database, execute the "StockExchangeScript.sql" in the database. It will create the "StockDB" database and the error log table. No other table will be created with the script since entity framework will deal with the creation of the rest of the objects. Make sure to update the filename path for the "StockDB" and the "StockDB_log" to a valid location for your environment.

The script is located in Jonathan_Smith-CSharpAssignment.zip\Source\Script.

Once the database is created, go to the "StockExchangeClient" project and modify the root web.config file and modidy the "connectionStrings" section to point to your server, then go to the "StockExchangeService" project and modify the root web.config file and modidy the "connectionStrings" section to point to your server. Since migrations is enabled for the project, go to the "StockExchangeClient" project and in the "Package Manager Console" run update-database, to force the update of the database.

2- Steps to prepare the source code to build properly

To build the projects the only thing needed is to set the "StockExchangeClient" project as the startup project. the project has a dependency on the web service hence it will compile the web service dll's as well.

3- Any Assumptions made and missing requirements that are not covered in the requirements

I added the change password functionality, just to make it a little bit more interactive.

among the constraints added is the length of the ticker code, the maximum length set is 5 characters, acording to a google search.

4- Steps to install the service and web application on IIS using the deploy packages

In order to install the packages, Web Deploy must be installed on the destination server. In addition, the version of web deploy on the destination server must be 9.0.1955.0 that corresponds to the version 3 of the web deploy.

you can install the packages in the following ways:
	-Use IIS Manager
	-Use the <projecname>.deploy.cmd file that visual studio creates with the package
	-Use Web Deploy commands directly from the command line or by excecuting PowerShell commands.

The deployment packages are located in Jonathan_Smith-CSharpAssignment.zip\Source\Deploy Packages.

5- Any feedback you may wish to give about improving the assignment

Perhaps instead of using asmx web services, wcf services could be used instead, since they are much more powerful and new technologies adapt to it much nicer than the old asmx web services.