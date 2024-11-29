USE [POINTOFSALE]
GO

/****** Object:  Table [dbo].[Purchase]    Script Date: 29/11/2024 15:55:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Purchase](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[P_Supplier_Name] [nvarchar](100) NULL,
	[Product_Name] [nvarchar](100) NULL,
	[Purchase_price] [decimal](10, 2) NULL,
	[Quantity] [int] NULL,
	[Supplier_Id] [int] NULL,
	[Product_Id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Purchase]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_Product] FOREIGN KEY([Product_Id])
REFERENCES [dbo].[Product] ([idProduct])
GO

ALTER TABLE [dbo].[Purchase] CHECK CONSTRAINT [FK_Purchase_Product]
GO

ALTER TABLE [dbo].[Purchase]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_Supplier] FOREIGN KEY([Supplier_Id])
REFERENCES [dbo].[Supplier] ([Supplier_Id])
GO

ALTER TABLE [dbo].[Purchase] CHECK CONSTRAINT [FK_Purchase_Supplier]
GO

create Trigger UpdateProductQuantity
on Purchase
After insert
as 
begin
update Product
Set Product.Quantity=Product.Quantity + inserted.Quantity
From Product 
inner join inserted on Product.idProduct=inserted.Product_Id;
end;


CREATE TRIGGER UpdateProductQuantity
ON Purchase
AFTER INSERT
AS
BEGIN
    UPDATE Product
    SET Product.Quantity = Product.Quantity + inserted.Quantity
    FROM Product
    INNER JOIN inserted ON Product.idProduct = inserted.Product_Id;
END;
;
-- Insert values into the Purchase table
INSERT INTO [dbo].[Purchase] (P_Supplier_Name, P_Supplier_Address, Product_Name, Purchase_price, Quantity, Supplier_Id, Product_Id)
VALUES
('Aurang Zeb Khan', 'PCSIR Near Back Side of UCP University', 'j.', 20000, 3, 20, 2)
select * from Supplier
select * from Product
select *from Purchase
delete from Purchase
where id=2
select * from Users


