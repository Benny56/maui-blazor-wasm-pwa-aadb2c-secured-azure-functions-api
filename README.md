# Secure Blazor sample solution with API

This .NET 7 sample solution is meant as a starter project, if you need

  1. An API based on Azure Functions and secured by Azure Active Directory B2C
  2. An installable Blazor Webassemby App ready for offline operations
  3. A MAUI Blazor Hybrid App for your mobile and desktop devices
  4. Shared Blazor pages
  5. A shared frontend-backend class library
  
# Getting started

## Configure Azure Active Directory B2C

  1. Create a new Azure Active Directory B2C tenant or use a excisting one. Copy the tenent name. You will later replace the placeholder <YOUR TENANT NAME> in appsettings.json
  
  2. Register a API application and expose at least one scope. Copy the Client-ID and the scope. Ypu will later replace the placeholders <YOUR API APPLICATION-ID (CLIENT-ID)> and <YOUR SCOPE> in appsettings.json
  
  3. Register a Web application as a Single-Page-Web-App with a redirect-URI of 'https://localhost/authentication/login-callback'.
  
  4. Grant API permissions and copy the Client-ID. You will later replace the placeholder <YOUR PROGRESIVE BLAZOR WEBASSEMBLY APPLICATION-ID (CLIENT-ID)> in appsettings.json.
  
  5. Register the MAUI Blazor App and configure two platforms
     - A Web with redirection-URI 'https://<YOUR TENANT NAME>.b2clogin.com/officeassistantb2c.onmicrosoft.com/oauth2/authresp'
     - A Mobile and Desktop Application with redirection-URIs 'msal<YOUR MAUI BLAZOR APPLICATION-ID (CLIENT-ID)>://auth' and 'https://<YOUR TENANT NAME>.b2clogin.com/oauth2/nativeclient'
     
  6. Grant API permissions and copy the Client-ID. You will later replace the placeholder <YOUR PROGRESIVE BLAZOR WEBASSEMBLY APPLICATION-ID (CLIENT-ID)> in appsettings.json.

## Configure both appsettings.json files

  1. Locate the wwwroot/appsettings.json file in the Sample.Web project and replace the placeholders.
  
     ```          
     {
        "Authentication": {
          "Authority": "https://<YOUR TENANT NAME>.b2clogin.com/tfp/<YOUR TENANT NAME>.onmicrosoft.com/<YOUR SIGN UP SIGN IN POLICY>",
          "ClientId": "YOUR PROGRESIVE BLAZOR WEBASSEMBLY APPLICATION-ID (CLIENT-ID)",
          "ValidateAuthority": false,
          "ApiBaseAddress": "YOUR API URL",
          "Scopes": [
            "https://<YOUR TENANT NAME>.onmicrosoft.com/<YOUR API APPLICATION-ID (CLIENT-ID)>/<YOUR SCOPE>"
          ]
        }
      }
      ```
  
  2. Locate the appsettings.json file in the Sample.Hybrid project and replace the placeholders.
  
     ```          
     {
        "Authentication": {
          "Authority": "https://<YOUR TENANT NAME>.b2clogin.com/tfp/<YOUR TENANT NAME>.onmicrosoft.com/<YOUR SIGN UP SIGN IN POLICY>",
          "ClientId": "YOUR MAUI BLAZOR APPLICATION-ID (CLIENT-ID)",
          "ValidateAuthority": false,
          "ApiBaseAddress": "YOUR API URL",
          "Scopes": [
            "https://<YOUR TENANT NAME>.onmicrosoft.com/<YOUR API APPLICATION-ID (CLIENT-ID)>/<YOUR SCOPE>"
          ]
        }
      }
      ```
  3. Make sure both appsettings.json files are identical except the values for 'ClientId'.   
  
  ## Congratulations
  
  You are ready to run the solution in your local development environment. To successfully access the data provided by the API you need to configure two start projects. The first one is always the Sample.API project the second one is either the Sample.Web or the Sample.Hybrid project.
  
