# EasyResult.WebExtensions

EasyResult.WebExtensions is a C# library that contains extensions to the EasyResult library designed to be used in Web projects.

## Installation: 

The Web Extensions library is also available as a NuGet package. To install it, open the Package Manager Console in Visual Studio and run the following command: ```Install-Package EasyResult.WebExtensions```.

## How to use

### Classes overview

```ResultHandler``` class that extends ```ResultHandler<T>``` being T ```ActionResult```.

### Handling results

#### UseDefaultWebClientErrorHandler

This method defines the default behaviour of the ```OnClientError``` handler.

#### UseDefaultWebServerErrorHandler

This method defines the default behaviour of the ```OnServerError``` handler.

#### UseWebDefaultErrorHandler

This method defines the default behaviour of the ```OnClientError``` and ```OnServerError``` handlers.

#### UseWebDefaultUnhittedHandler

This method defines the default behaviour of the ```OnUnhittedHandler``` handler.

#### UseWebDefaultsHandler

This method defines the default behaviour of the ```OnClientError```, ```OnServerError``` and ```OnUnhittedHandler``` handlers.