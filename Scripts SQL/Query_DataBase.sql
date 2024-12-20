
create database POINTOFSALE

go

use POINTOFSALE

go

create table Menu(
[idMenu] int primary key identity(1,1),
[description] varchar(30),
[idMenuParent] int references Menu(idMenu),
[icon] varchar(30),
[controller] varchar(30),
[pageAction] varchar(30),
[isActive] bit,
[registrationDate] datetime default getdate()
)

go


create table Rol(
[idRol] int primary key identity(1,1),
[description] varchar(30),
[isActive] bit,
[registrationDate] datetime default getdate()
)
 go
 
 create table RolMenu(
 [idRolMenu] int primary key identity(1,1),
 [idRol] int references Rol(idRol),
 [idMenu] int references Menu(idMenu),
 [isActive] bit,
 [registrationDate] datetime default getdate()
 )

 go


create table Users(
[idUsers] int primary key identity(1,1),
[name] varchar(50),
[email] varchar(50),
[phone] varchar(50),
[idRol] int references Rol(idRol),
[password] varchar(100),
[photo] varbinary(max),
[isActive] bit,
[registrationDate] datetime default getdate()
)

go

create table Category(
[idCategory] int primary key identity(1,1),
[description] varchar(50),
[isActive] bit,
[registrationDate] datetime default getdate()
)

go

create table Product(
[idProduct] int primary key identity(1,1),
[barCode] varchar(50),
[brand] varchar(50),
[description] varchar(100),
[idCategory] int references Category(idCategory),
[quantity] int,
[price] decimal(10,2),
[photo] varbinary(max),
[isActive] bit,
[registrationDate] datetime default getdate()
)

go

create table CorrelativeNumber(
[idCorrelativeNumber] int primary key identity(1,1),
[lastNumber] int,
[quantityDigits] int,
[management] varchar(100),
[dateUpdate] datetime
)

go


create table TypeDocumentSale(
[idTypeDocumentSale] int primary key identity(1,1),
[description] varchar(50),
[isActive] bit,
[registrationDate] datetime default getdate()
)

go

create table Sale(
[idSale] int primary key identity(1,1),
[saleNumber] varchar(6),
[idTypeDocumentSale] int references TypeDocumentSale(idTypeDocumentSale),
[idUsers] int references Users(idUsers),
[customerDocument] varchar(10),
[clientName] varchar(20),
[Subtotal] decimal(10,2),
[totalTaxes] decimal(10,2),
[total] decimal(10,2),
[registrationDate] datetime default getdate()
)

go


create table DetailSale(
[idDetailSale] int primary key identity(1,1),
[idSale] int references Sale(idSale),
[idProduct] int,
[brandProduct] varchar(100),
[descriptionProduct] varchar(100),
[categoryProducty] varchar(100),
[quantity] int,
[price] decimal(10,2),
[total] decimal(10,2)
)

go

create table Negocio(
idNegocio int primary key,
urlLogo varchar(500),
nombreLogo varchar(100),
numeroDocumento varchar(50),
nombre varchar(50),
correo varchar(50),
direccion varchar(50),
telefono varchar(50),
porcentajeImpuesto decimal(10,2),
simboloMoneda varchar(5)
)
go
CREATE TABLE [dbo].[Supplier](
	[Supplier_Id] [int] IDENTITY(1,1) NOT NULL,
	[Supplier_Name] [nvarchar](100) NULL,
	[Supplier_Address] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Supplier_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
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


