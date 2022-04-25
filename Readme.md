# Library Application

## File Structure:
- Library_DB.bak file is located in App_Data folder 
- Library_Application, packages, Library-Application.sln are project files..

## Setup
- Download from Github
- Open the App_Data folder and restore the .bak file in the sql server.
- Open Library-Application.sln
- Inside project open Web.config file and set the database connection string.

## User Guide

- Home page : Includes Signup Login and Admin button ("/")
- SignUp :  Create account and go back to login page  ("Home/Signup")
- Login Page : Login with email and it will redirect you to the user page ("Home/Login")
- User Page :  you can rent a book ("/User")
- Rented page : you return borrow book ("User/Rented")
- Admin Page : Includes two links Book List And Author List ("Home/Admin")
- Book List Page  :  You can perfrom operations like Add Edit and Delete Books list ("BookData")
- Author List Page : You can perfrom operations like Add Edit and Delete Author list ("Author")
