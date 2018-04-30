CREATE PROCEDURE [dbo].[GetPeople]
	@PageSize int = 25,
	@PageNumber int = 1,	
	@OrderBy nvarchar(255) = 'PersonId',
	@OrderAsc bit = 1,
	@TotalRows int OUTPUT
AS
	DECLARE @table TABLE (
		PersonId int,
		FirstName varchar(255),
		LastName varchar(255),
		MiddleName varchar(255)
	);

	IF @PageNumber < 1
		SET @PageNumber = 1;

	INSERT INTO @table
	SELECT  [PersonId]
			,[FirstName]
			,[LastName]
			,[MiddleName]
	FROM [ApiSample].[dbo].[Person];

	IF @OrderBy = 'PersonId'
		BEGIN
			SELECT * FROM @table
		ORDER BY 
			CASE WHEN @OrderAsc = 1 THEN CASE WHEN @OrderBy = 'PersonId' THEN PersonId END END ASC,
			CASE WHEN @OrderAsc <> 1 THEN CASE WHEN @OrderBy = 'PersonId' THEN PersonId END END DESC
		OFFSET ((@PageNumber - 1) * @PageSize) ROWS
		FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE
	SELECT * FROM @table
	ORDER BY
		--CASE WHEN @OrderAsc = 1 THEN CASE WHEN @OrderBy = 'PersonId' THEN PersonId END END ASC,
		--CASE WHEN @OrderBy = 'PersonId' AND @OrderAsc <> 1 THEN PersonId END DESC,
		CASE 
		WHEN @OrderAsc = 1 THEN
			CASE 
				WHEN @OrderBy = 'FirstName' THEN FirstName
				WHEN @OrderBy = 'LastName' THEN LastName
				WHEN @OrderAsc= 'MiddleName' THEN MiddleName
			END
		END ASC,
		CASE 
		WHEN @OrderAsc <> 1 THEN
			CASE 
				WHEN @OrderBy = 'FirstName' THEN FirstName
				WHEN @OrderBy = 'LastName' THEN LastName
				WHEN @OrderAsc= 'MiddleName' THEN MiddleName
			END
		END DESC
	OFFSET ((@PageNumber - 1) * @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY
