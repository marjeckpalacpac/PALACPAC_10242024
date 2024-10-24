**Overview**

This project demonstrates a secure RESTful web service for file processing, supporting either CSV or JSON file formats. 
It includes basic tracking of files processed and is secured using an API Key-based authentication mechanism.

**Features**
1. Secure Web Service: ASP.NET Core-based service with API Key authentication.
2. File Processing:
      CSV: Calculates the average of a specific column.
      JSON: Performs data transformation by filtering based on a condition.
3. File Tracking: Logs basic details such as filename and processing time.
4. Documentation: Includes instructions for building, running, and using the service.


**API Endpoints**
File Upload
Endpoint: /api/fileProcessing

Method: POST
Description: Upload a CSV or JSON file for processing.

Headers:
api-key: <api-key> (required)

Request Body:
multipart/form-data containing the file.

Sample Request:
curl -X POST "http://localhost:7247/api/fileProcessing" -H "api-key: your-api-key" -F "file=@path/file.csv"

Response: 
200 OK: Returns the processing result. 
400 Bad Request: Returns an error if the file format is invalid or a required field is missing. 
500 Server Error: Returns an error that occured and catch in the server.


**Instructions for Building and Running the Service** 
**Prerequisites** 
Visaul Studio 2022 
Step 1: Clone the Repository
    Repository location: https://github.com/marjeckpalacpac/PALACPAC_10242024.git

Step 2: Build and Run Locally 
    Select "Build Solution" or ctrl+shift+b to build the application
    Click "Start" to run the application
    The service will be available at https://localhost:7247

Step 3: Use Postman
    Method: Post
    API URL: https://localhost:7247/api/FileProcessing
    Headers:
        Key: API-Key
        Value: ihlu_7t8gQ_GjFRB4tr3uNUKyKEYrtvWTqVvnvzWBGI
    Body:
        form-data
            Key: file
            Type: File
            Value: @path/file.csv
    Send the request
