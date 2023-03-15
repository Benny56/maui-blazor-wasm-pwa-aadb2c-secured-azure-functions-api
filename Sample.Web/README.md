# OfficeAssistant.Web

Das Project ist eine eigenständige, progressive Blazor Web Assembly Anwendung mit Absicherung über Azure Active Directory B2C.

# Setup

## Vorlage

Das Setup dieses Projektes folgt der Anleitung [Sichern einer eigenständigen Blazor WebAssembly-App in ASP.NET Core mit Azure Active Directory B2C](https://learn.microsoft.com/de-de/aspnet/core/blazor/hosting-models?view=aspnetcore-3.1#blazor-webassembly).
Der angegebene .NET CLI-Befehl ist jedoch um einige Parameter ergänzt: '--pwa' ergänzt, um eine Progressive Web App zu erhalten und Angaben zu einer bereits existierenden auf Azure Functions basierenden API.

.NET CLI-Befehl:

	dotnet new blazorwasm --pwa -au IndividualB2C --aad-b2c-instance "{AAD B2C INSTANCE}" -ssp "{SIGN UP OR SIGN IN POLICY}" --client-id "{CLIENT ID}" --domain "{TENANT DOMAIN}" --app-id-uri "{APP-ID-URI}" --default-scope "{SCOPE}" --called-api-url "{CALLED API URL}" --called-api-scopes "{CALLED API Scopes}" --name "{APP NAME}" --output "{OUTDIR}"

Beispiel:

	**Platzhalter			Name im Azure-Portal						Wert**
	{AAD B2C INSTANCE}		Instanz								https://officeassistantb2c.b2clogin.com/
	{SIGN UP OR SIGN IN POLICY}	Benutzerflow für die Registrierung oder Anmeldung	B2C_1_susi
	{CLIENT ID}				Anwendungs-ID (Client)						33b59da3-4e64-4ebe-9392-715e82233458
	{TENANT DOMAIN}			Domänenname								officeassistantb2c.onmicrosoft.com
	{APP-ID-URI}			Anwendungs-ID-URI							https://officeassistantb2c.onmicrosoft.com/api
	{SCOPE}				Bereiche								api.access
	{CALLED API URL}												http://localhost:7093
	{CALLED API Scopes}		Bereiche								https://officeassistantb2c.onmicrosoft.com/api/api.access        
	{APP NAME}				Anzeigename								OfficeAssistant.Web
	{OUTDIR}													OfficeAssistant.Web


.NET CLI-Befehl:

	dotnet new blazorwasm --pwa -au IndividualB2C --aad-b2c-instance "https://officeassistantb2c.b2clogin.com/" -ssp "B2C_1_susi" --client-id "33b59da3-4e64-4ebe-9392-715e82233458" --domain "officeassistantb2c.onmicrosoft.com" --app-id-uri "https://officeassistantb2c.onmicrosoft.com/api" --default-scope "api.access" --called-api-url "http://localhost:7093" --called-api-scopes "https://officeassistantb2c.onmicrosoft.com/api/api.access" --name "OfficeAssistant.Web" --output "OfficeAssistant.Web"


##**Fehler**

Der Befehl scheint nichts zu bringen. Es gibt keinen Zugriff auf die Api oder unterstützende Methoden. 