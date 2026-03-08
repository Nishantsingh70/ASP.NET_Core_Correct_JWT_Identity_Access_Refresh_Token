# ASP.NET_Core_Correct_JWT_Identity_Access_Refresh_Token
This is JWT implementation with access and refresh token for authentication.

Suppose you have 25 years of experience in ASP.NET Core, JWT Tokens, Entity Framework, SQL Server, AWS Secrets Manager, SQL and JWT Security best practices. Now you have to understand the below ASP.NET Core requirement and Provide the best code which will follow all the Industry Enterprise best practices? 

Please implement all the industry best practice and also use the repository pattern to implement everything. 

1. We are storing username and password Hash in DB to validate the user and generate the access token and refresh token for valid user. 
2. We must need to invalidate the old refresh token after calling it to generate new refresh token. 
3. Refresh token call will generate the access token and new refresh token which will store in DB also. 
4. We are using the Role based Authentication in this and storing the role along with username and password hash (not directly storing the password in db). 
5. Please use ASP.NET Identity PasswordHasher hashing algorithm because it is best for ASP.NET Core applications How to authenticate/validate the Generated JWT token in asp.net core application? 
6. Please take of this that client will use username and password and then you need to convert the password in hash format and compare it with already stored password hash in the database.
7. Please assume that JWT Issuer, Audience and Secret Key stored at AWS Secrets Manager.
8. Give all the Model class with proper Data annotation.
9. Apply Role based Authentication on some of the action methods.

Please explain the setup wise process with proper example and explanation and give the most appropriate code snippets as you have 20 years of experience in asp.net core application development? Please add the refresh token concept also in it & give most simplest code which can do everything here.
