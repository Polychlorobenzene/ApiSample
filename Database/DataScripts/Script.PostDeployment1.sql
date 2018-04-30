/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
USE ApiSample

INSERT INTO Person (FirstName,LastName,MiddleName)
SELECT 'F'+SUBSTRING(CONVERT(varchar(36),NEWID()),0,5), 'L' + SUBSTRING(CONVERT(varchar(36),NEWID()),0,5), SUBSTRING(CONVERT(varchar(36),NEWID()),3,1)
GO 10000