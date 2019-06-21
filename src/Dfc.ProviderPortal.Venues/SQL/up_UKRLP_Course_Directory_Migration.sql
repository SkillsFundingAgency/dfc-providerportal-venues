
DROP PROCEDURE dbo.dfc_TribalToCourseDirectoryVenueMigration
GO

CREATE PROCEDURE dbo.dfc_TribalToCourseDirectoryVenueMigration

AS
BEGIN
  SELECT V.VenueName AS VENUE_NAME
	  ,V.VenueId AS VENUE_ID
	  ,P.Ukprn AS UKPRN
	  ,V.ProviderId AS PROVIDER_ID
	  ,V.ProviderOwnVenueRef AS PROV_VENUE_ID
      ,V.Email AS EMAIL
      ,V.Website AS WEBSITE
	  ,V.Telephone AS PHONE
	  ,A.AddressLine1 AS ADDRESS_1
	  ,A.AddressLine2 AS ADDRESS_2
	  ,A.Town AS TOWN
	  ,A.County AS COUNTY
	  ,A.Postcode AS POSTCODE
	  ,GL.Lat AS Latitude
	  ,GL.Lng AS Longitude
	  ,l.LocationId
	  ,V.CreatedDateTimeUtc AS DATE_CREATED
	  ,V.ModifiedDateTimeUtc AS DATE_UPDATE
  FROM Tribal.Venue V
  LEFT OUTER JOIN Tribal.Provider P ON V.ProviderId = P.ProviderId
  LEFT OUTER JOIN Tribal.Address A ON V.AddressId = A.AddressId
  LEFT OUTER JOIN Tribal.Location l ON A.AddressId = l.AddressId
  LEFT OUTER JOIN Tribal.GeoLocation GL ON A.Postcode = GL.Postcode
  WHERE V.RecordStatusId = 2 AND P.RecordStatusId = 2

END
GO

