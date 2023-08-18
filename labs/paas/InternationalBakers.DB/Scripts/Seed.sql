
if not exists(select 1 from dbo.stores) 
BEGIN

--INSERT INTO dbo.DatabaseServers(DatabaseServer, DatabaseName, Region) values ('internationalcookiesq2bpnesdylzbk.database.windows.net', 'internationalcookies-db-we', 'WE')
--INSERT INTO dbo.DatabaseServers(DatabaseServer, DatabaseName, Region) values ('internationalcookiesq2bpneshskdj.database.windows.net', 'internationalcookies-db-bs', 'BS')
--INSERT INTO dbo.DatabaseServers(DatabaseServer, DatabaseName, Region) values ('internationalcookiesq2bpnesdjkhf.database.windows.net', 'internationalcookies-db-cus', 'CUS')
--INSERT INTO dbo.DatabaseServers(DatabaseServer, DatabaseName, Region) values ('internationalcookiesq2bppqoeyns.database.windows.net', 'internationalcookies-db-sea', 'SEA')


INSERT INTO dbo.cookies (ImageUrl, [Name], Price) values ('https://ibcdnep01.azureedge.net/cookie-cc.jpg', 'Chololate Chip', 1.2)
INSERT INTO dbo.cookies (ImageUrl, [Name], Price) values ('https://ibcdnep01.azureedge.net/cookie-bc.jpg', 'Butter Cookie', 1.0)
INSERT INTO dbo.cookies (ImageUrl, [Name], Price) values ('https://ibcdnep01.azureedge.net/cdn/cookie-mc.jpg', 'Macaroons', 0.9)

INSERT INTO dbo.stores (Country, [Name]) values ('Netherlands', 'Holland Cookies')
INSERT INTO dbo.stores (Country, [Name]) values ('United States', 'Excellent Cookies')
INSERT INTO dbo.stores (Country, [Name]) values ('Brazil', 'Yummie Cookies')
INSERT INTO dbo.stores (Country, [Name]) values ('Singapore', 'Lovely Cookies')

INSERT INTO dbo.[Orders] ([Date], Price, StoreId) values ('2022-01-15', 20, 1)
INSERT INTO dbo.[OrderLines] (CookieId, OrderId, Quantity) values (1, 1, 3)


END