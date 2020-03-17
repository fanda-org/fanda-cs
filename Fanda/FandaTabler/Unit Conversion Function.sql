CREATE FUNCTION dbo.ufn_ConvertUnit
(
	@UnitFrom uniqueidentifier,
	@UnitTo uniqueidentifier,
	@UnitValue numeric(12,5)
)
RETURNS numeric(12,5)
 
BEGIN
	--DECLARE @result numeric(12,5)=@UnitValue

	-- Operator, Factor
	IF EXISTS(SELECT TOP(1) 1 FROM dbo.UnitConversions WITH(NOLOCK) WHERE FromUnitId=@UnitFrom AND ToUnitId=@UnitTo)
		SELECT @UnitValue = 
		CASE Operator
		WHEN '*' THEN (@UnitValue * Factor)
		WHEN '/' THEN (@UnitValue / Factor)
		WHEN '+' THEN (@UnitValue + Factor)
		WHEN '-' THEN (@UnitValue - Factor)
		ELSE @UnitValue
		END
		FROM dbo.UnitConversions WITH(NOLOCK)
		WHERE FromUnitId=@UnitFrom AND ToUnitId=@UnitTo
		ORDER BY ROW_NUMBER() OVER( ORDER BY CalcStep )
	ELSE IF EXISTS(SELECT TOP(1) 1 FROM dbo.UnitConversions WITH(NOLOCK) WHERE FromUnitId=@UnitTo AND ToUnitId=@UnitFrom)
		SELECT @UnitValue = 
		CASE Operator 
		WHEN '*' THEN (@UnitValue / Factor)
		WHEN '/' THEN (@UnitValue * Factor)
		WHEN '+' THEN (@UnitValue - Factor)
		WHEN '-' THEN (@UnitValue + Factor)
		ELSE @UnitValue
		END
		FROM dbo.UnitConversions WITH(NOLOCK)
		WHERE FromUnitId=@UnitTo AND ToUnitId=@UnitFrom
		ORDER BY ROW_NUMBER() OVER( ORDER BY CalcStep DESC )
	--ELSE
	--	SET @result=null

	RETURN @UnitValue;
END

go

CREATE FUNCTION dbo.ufn_GetChildUnit
(
	@UnitId uniqueidentifier
)
RETURNS uniqueidentifier
WITH SCHEMABINDING
AS
BEGIN
	DECLARE @ChildUnitId uniqueidentifier

	;WITH Unit_cte AS
	(
		SELECT FromUnitId, ToUnitId, [Level]=1
		FROM dbo.UnitConversions
		WHERE FromUnitId=@UnitId
		UNION ALL
		SELECT u.FromUnitId, u.ToUnitId, [Level]+1
		FROM dbo.UnitConversions u
		INNER JOIN Unit_cte ucte ON ucte.ToUnitId=u.FromUnitId
	)
	SELECT TOP 1 @ChildUnitId=ToUnitId  
	FROM Unit_cte
	ORDER BY [Level] DESC

	RETURN CASE WHEN @ChildUnitId IS NULL THEN @UnitId ELSE @ChildUnitId END
END
go

insert into Units(UnitId, OrgId, Code, [Name], Active, DateCreated) values(NEWID(), 'AAEFE9D0-A36E-46E0-1E19-08D5EA042032', 'DOZ', 'Dozen', 1, GETDATE())
insert into Units(UnitId, OrgId, Code, [Name], Active, DateCreated) values(NEWID(), 'AAEFE9D0-A36E-46E0-1E19-08D5EA042032', 'KG', 'Kilogram', 1, GETDATE())
insert into Units(UnitId, OrgId, Code, [Name], Active, DateCreated) values(NEWID(), 'AAEFE9D0-A36E-46E0-1E19-08D5EA042032', 'LB', 'Pounds', 1, GETDATE())
insert into Units(UnitId, OrgId, Code, [Name], Active, DateCreated) values(NEWID(), 'AAEFE9D0-A36E-46E0-1E19-08D5EA042032', 'CEL', 'Celsius', 1, GETDATE())
insert into Units(UnitId, OrgId, Code, [Name], Active, DateCreated) values(NEWID(), 'AAEFE9D0-A36E-46E0-1E19-08D5EA042032', 'FAH', 'Fahrenheit', 1, GETDATE())

INSERT INTO UnitConversions(ConversionId, OrgId, FromUnitId, ToUnitId, CalcStep, Operator, Factor, Active) VALUES
(NEWID(), 'AAEFE9D0-A36E-46E0-1E19-08D5EA042032', '5016A2BD-6B52-4486-B85A-08D5EFDE4751', 'C79E8B38-68EC-4961-B770-489D1814118E', 1, '*', 12, 1),
(NEWID(), 'AAEFE9D0-A36E-46E0-1E19-08D5EA042032', 'FA832EC8-87AD-4963-A46A-2F447E5AAF64', 'EE9978D0-A68A-42E7-BEB5-C3B945DAD73F', 1, '*', 2.2, 1),
(NEWID(), 'AAEFE9D0-A36E-46E0-1E19-08D5EA042032', '8CA5E21A-4C6D-44CB-92DB-0BC3D1DD17C8', 'A6D36ABA-F5B6-48D6-9A1E-7F94C3F6E3E8', 1, '*', 1.8, 1),
(NEWID(), 'AAEFE9D0-A36E-46E0-1E19-08D5EA042032', '8CA5E21A-4C6D-44CB-92DB-0BC3D1DD17C8', 'A6D36ABA-F5B6-48D6-9A1E-7F94C3F6E3E8', 2, '+', 32, 1)
go

select u1.[Name] UnitFrom, u2.[Name] UnitTo, CalcStep, Operator, Factor
from UnitConversions uc 
inner join Units u1 on uc.FromUnitId=u1.UnitId
inner join Units u2 on uc.ToUnitId=u2.UnitId
--where uc.FromUnitId=6 and uc.ToUnitId=7
order by FromUnitId,ToUnitId,CalcStep
go

SELECT FromUnitId, ToUnitId, [Level]=1
FROM UnitConversion
WHERE FromUnitId=1

--EXEC usp_GetChildUnit 9
SELECT dbo.ufn_GetChildUnit(1)

SELECT * FROM Inventory
SELECT * FROM InvoiceItem
go

select * from Inventory
go

SELECT dbo.ufn_GetChildUnit(1)
SELECT dbo.ufn_ConvertUnit(1, 3, 40)
SELECT TOP 1 Quantity, UnitId FROM Inventory WITH(NOLOCK) WHERE Code='MOUSE' AND ProductId=5
GO

-- EXEC usp_SaveInventory 'MOUSE', 5, -8, 1
ALTER PROCEDURE usp_SaveInventory
(
	@Code nvarchar(15)
	,@ProductId int
	,@Quantity money
	,@UnitId int
	,@Description nvarchar(255) = NULL
	,@ExpiryDate date = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @NewQty money
	DECLARE @NewUnitId int

	SELECT @NewUnitId = dbo.ufn_GetChildUnit(@UnitId)
	SELECT @NewQty = dbo.ufn_ConvertUnit(@UnitId, @NewUnitId, @Quantity)

	IF NOT EXISTS(SELECT TOP 1 'A' FROM Inventory WITH(NOLOCK) WHERE Code=@Code AND ProductId=@ProductId AND UnitId=@NewUnitId)
		INSERT INTO Inventory(Code, ProductId, Quantity, UnitId, [Description], ExpiryDate)
		VALUES(@Code, @ProductId, @NewQty, @NewUnitId, @Description, @ExpiryDate)
	ELSE
	BEGIN
		-- Already exists with different unit id
		DECLARE @dbQty money
		DECLARE @dbUnitId int
		SELECT TOP 1 @dbQty=Quantity, @dbUnitId=UnitId FROM Inventory WITH(NOLOCK) WHERE Code=@Code AND ProductId=@ProductId
		
		IF @dbUnitId <> @NewUnitId
		BEGIN
			DECLARE @NewDbUnitId int
			DECLARE @NewDbQty money
			SELECT @NewDbUnitId = dbo.ufn_GetChildUnit(@dbUnitId)
			SELECT @NewDbQty = dbo.ufn_ConvertUnit(@dbUnitId, @NewUnitId, @dbQty)
			
			UPDATE Inventory WITH(ROWLOCK) SET Quantity=@NewDbQty, UnitId=@NewDbUnitId 
			WHERE Code=@Code AND ProductId=@ProductId AND UnitId=@dbUnitId
		END

		-- Common update
		UPDATE Inventory WITH(ROWLOCK) SET Quantity += @NewQty, UnitId=@NewUnitId,
		[Description]=CASE WHEN @Description IS NULL THEN [Description] ELSE @Description END,
		ExpiryDate=CASE WHEN @ExpiryDate IS NULL THEN ExpiryDate ELSE @ExpiryDate END
		WHERE Code=@Code AND ProductId=@ProductId AND UnitId=@NewUnitId
	END

	SET NOCOUNT OFF;
END
GO

CREATE PROCEDURE usp_GetInventory
(
	@ProductId int
	,@Code varchar(15)
	,@CompanyId int
	,@PeriodId int
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.Id ProductId
	,CASE WHEN ISNULL(i.Code,'')='' THEN p.Code ELSE I.Code END Code
	, p.[Name] ProductName
	,CASE WHEN ISNULL(i.[Description],'')='' THEN p.[Description] ELSE i.[Description] END [Description]
	,p.CostPrice, p.ProfitPercent, p.SellingPrice, p.TaxPercent
	,p.Surcharge, p.Freight
	,p.MinimumQuantity, p.MaximumQuantity, p.ReorderLevelQuantity
	,i.ExpiryDate, i.Quantity, I.UnitId
	FROM Inventory i WITH(NOLOCK)
	INNER JOIN Product p WITH(NOLOCK) ON i.ProductId=p.Id

	SET NOCOUNT OFF;
END
GO

select * from Company

SELECT * FROM Product

SELECT * FROM Invoice
SELECT * FROM InvoiceItem

SELECT * FROM Inventory

select * from AccountCategory
select * from Account
select * from [Period] where CompanyId=2

-- INSERT INTO Invoice(Code,[Date],AccountId,TranAccountId,[InvoiceType],PeriodId) VALUES('M-001','2016-11-01',1,2,'Purchase',7)

SELECT 1
FROM Invoice i WITH(NOLOCK)
INNER JOIN (InvoiceItem ii WITH(NOLOCK) INNER JOIN Inventory inv WITH(NOLOCK) ON ii.InventoryId=inv.Id) ON i.Id=ii.InvoiceId
INNER JOIN (Company c WITH(NOLOCK) INNER JOIN [Period] p WITH(NOLOCK) ON c.Id=p.CompanyId) ON i.PeriodId=p.Id


INSERT INTO Product(Code, [Name], LevelNumber, IndexNumber, IsGroup, UnitId, CostPrice, SellingPrice, TaxPercent, Surcharge, Freight, ProfitPercent,
MinimumQuantity, MaximumQuantity, ReorderLevelQuantity, CompanyId, Active)
VALUES('MOUSE', 'Mouse', 1, 1, 0, 1, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0, 0, 0, 2, 1)



insert into Unit values('DOZ','Dozen','',12,null,0,0,null,2,1)
insert into Unit values('KG','Kilogram','',0,null,0,0,null,2,1)
insert into Unit values('LB','Pounds','',0,null,0,0,null,2,1)
insert into Unit values('CEL','Celsius','',0,null,0,0,null,2,1)
insert into Unit values('FAH','Fahrenheit','',0,null,0,0,null,2,1)

insert into UnitConversion values (4,5,1,'*',2.2, 1)
insert into UnitConversion values (6,7,1,'*',1.8, 1)
insert into UnitConversion values (6,7,2,'+',32, 1)
insert into UnitConversion values (1,3,1,'/',12, 1)
insert into UnitConversion values (3,1,1,'*',12, 1)
insert into UnitConversion values (8, 1, 1, '*' , 10, 1)
insert into UnitConversion values (9, 8, 1, '*' , 10, 1)


select * from Unit
select * from UnitConversion

select u1.[Name] UnitFrom, u2.[Name] UnitTo, CalcStep, Operator, Factor
from UnitConversion uc 
inner join Unit u1 on uc.FromUnitId=u1.Id
inner join Unit u2 on uc.ToUnitId=u2.Id
where uc.FromUnitId=6 and uc.ToUnitId=7
order by CalcStep
go

--drop function dbo.ufn_UnitConv

select uc.FromUnitId, uc.ToUnitId, u1.[Name] UnitFrom, u2.[Name] UnitTo, CalcStep, Operator, Factor
from UnitConversion uc 
inner join Unit u1 on uc.FromUnitId=u1.Id
inner join Unit u2 on uc.ToUnitId=u2.Id
--where uc.UnitFrom=6 and uc.UnitTo=7
--order by CalcStep

select * from Unit
select * from UnitConversion



SELECT dbo.ufn_ConvertUnit(3, 1, 12)
SELECT dbo.ufn_ConvertUnit(1, 3, 12)

select convert(money, 10 * 2.20462)
SELECT dbo.ufn_ConvertUnit(4, 5, 10)

SELECT dbo.ufn_ConvertUnit(6, 7, 100), dbo.ufn_ConvertUnit(7, 6, 212)

select dbo.ufn_ConvertUnit(6, 7, 47) -- cel to fah
select dbo.ufn_ConvertUnit(7, 6, 35) -- fah to cel

select dbo.ufn_ConvertUnit(3, 1, 2) -- doz to no.
select dbo.ufn_ConvertUnit(1, 3, 25) -- no. to doz

select 0.08333*12

-- eval
exec('select 0.08333' + '* 12')
go

SELECT dbo.ufn_GetChildUnit(3)
