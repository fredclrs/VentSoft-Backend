-- ============================================================================
-- Script de creacion COMPLETO de la base VentSoft, generado automaticamente
-- desde la base de datos real el 08-sep-2026 (columnas, tipos, PK, FK, UNIQUE
-- y DEFAULT tal cual estan hoy) — reemplaza la version anterior de este
-- archivo, que quedo desactualizada porque varias tablas y columnas se
-- fueron agregando con ALTER_*.sql / CREATE_*.sql sueltos a lo largo del
-- desarrollo (Devoluciones/Cambios, Pago en especie, Configuracion del
-- negocio, caja/unidad, costo por venta, etc.) sin volver a tocar este
-- archivo maestro.
--
-- Uso: para levantar una base NUEVA desde cero (por ejemplo, para otro
-- negocio/instalacion), correr este script entero en SSMS contra un
-- servidor sin la base VentSoft creada todavia. Si la base YA existe, NO
-- correr esto — usar los ALTER_*.sql puntuales (o pedir uno nuevo) para el
-- cambio que haga falta, nunca este script completo.
--
-- Como se genero: se consulto sys.tables/sys.columns/sys.foreign_keys/
-- sys.key_constraints de la base real (no se escribio a mano), y se
-- verifico ejecutandolo contra una base de prueba antes de guardarlo.
-- Si se agregan mas tablas/columnas en el futuro, hay que repetir ese
-- proceso para que este archivo no se vuelva a desactualizar.
-- ============================================================================

create database VentSoft
go
use VentSoft
go

create table Articulo (
    Id int identity(1,1) primary key not null,
    Codigo varchar(40) not null,
    Descripcion varchar(100) null,
    Tamano varchar(20) not null,
    UnidadMedida varchar(20) null,
    Fraccion float not null,
    Precio float not null,
    Costo float not null,
    StockMinimo float null,
    StockIdeal float null,
    Imagen varchar(30) null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null,
    IdFamilia int not null,
    IdPromocion int null,
    PrecioUnidadSuelta float null,
    MargenGanancia float null
);
go

create table ArticuloCaracteristica (
    Id int identity(1,1) primary key not null,
    IdArticulo int not null,
    IdCaracteristica int not null,
    Valor varchar(100) not null,
    constraint UQ_ArticuloCaracteristica unique (IdArticulo, IdCaracteristica)
);
go

create table Caracteristica (
    Id int identity(1,1) primary key not null,
    NombreCaracteristica varchar(80) not null,
    Descripcion varchar(150) null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null
);
go

create table Cliente (
    Id int identity(1,1) primary key not null,
    Nombre varchar(50) not null,
    DocumentoIdentidad varchar(20) not null,
    PersonaContacto varchar(50) null,
    Direccion varchar(100) null,
    Zona varchar(100) null,
    Telefono varchar(20) null,
    Correo varchar(30) null,
    Nota varchar(100) null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActilizado datetime null,
    FechaBaja datetime null,
    SaldoAFavor float default ((0)) not null
);
go

create table Cobro (
    Id int identity(1,1) primary key not null,
    DeudaActual float not null,
    Fecha date not null,
    Monto float not null,
    Recibo varchar(30) null,
    Nota varchar(150) null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null,
    IdCliente int not null,
    IdUsuario int not null
);
go

create table Compra (
    Id int identity(1,1) primary key not null,
    Fecha date not null,
    Referencias varchar(100) null,
    Total float not null,
    Pagado float not null,
    PorPagar float not null,
    Nota varchar(150) null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null,
    IdUsuario int not null,
    IdProveedor int not null
);
go

create table ConfiguracionEmpresa (
    Id int identity(1,1) primary key not null,
    Nombre varchar(100) not null,
    Moneda varchar(10) default ('Bs.') not null,
    PermiteVentaACredito bit default ((1)) not null,
    PermiteCompraACredito bit default ((1)) not null,
    RedondearPreciosEnteros bit default ((0)) not null,
    IdClientePorDefecto int null,
    IdProveedorPorDefecto int null
);
go

-- Si la base ya existía de antes (ya habías corrido este script una vez) y le faltan estas
-- columnas, corré la que corresponda para agregarla sin perder los datos de la tabla:
-- alter table ConfiguracionEmpresa add PermiteCompraACredito bit default ((1)) not null;
-- go
-- alter table ConfiguracionEmpresa add RedondearPreciosEnteros bit default ((0)) not null;
-- go
-- alter table FormaDePago add EsEfectivo bit default ((0)) not null;
-- go
-- alter table Venta add IdFormaDePago int null;
-- go
-- alter table Venta add constraint FK_Venta_FormaDePago foreign key (IdFormaDePago) references FormaDePago (Id) on delete set null;
-- go
-- create table AjusteStock (
--     Id int identity(1,1) primary key not null,
--     Fecha date not null,
--     Tipo varchar(10) not null,
--     Cantidad int not null,
--     Motivo varchar(200) not null,
--     IdArticulo int not null,
--     IdUsuario int not null,
--     Estado char(2) not null,
--     UserRegistro varchar(30) null,
--     UserActualizado varchar(30) null,
--     UserBaja varchar(30) null,
--     FechaRegistro datetime null,
--     FechaActualizado datetime null,
--     FechaBaja datetime null
-- );
-- go
-- alter table AjusteStock add constraint FK_AjusteStock_Articulo foreign key (IdArticulo) references Articulo (Id);
-- go
-- alter table AjusteStock add constraint FK_AjusteStock_Usuario foreign key (IdUsuario) references Usuario (Id);
-- go

create table DetalleCambioVenta (
    Id int identity(1,1) primary key not null,
    Cantidad int not null,
    PrecioUnitario float not null,
    SubTotal float not null,
    IdDevolucion int not null,
    IdArticulo int not null
);
go

create table DetalleCompra (
    Id int identity(1,1) primary key not null,
    Cantidad int not null,
    CostoUnitario money not null,
    SubtoTotal money not null,
    Pagado money not null,
    Lote varchar(30) null,
    FechaVencimiento date null,
    IdCompra int not null,
    IdArticulo int not null
);
go

create table DetalleDevolucionVenta (
    Id int identity(1,1) primary key not null,
    Cantidad int not null,
    PrecioUnitario float not null,
    SubTotal float not null,
    Vendible bit not null,
    IdDevolucion int not null,
    IdDetalleVenta int not null,
    IdArticulo int not null
);
go

create table DetalleVenta (
    Id int identity(1,1) primary key not null,
    Cantidad int not null,
    PrecioUnitario float not null,
    DescuentoMonetario float null,
    DescuentoPorcentaje float null,
    SubTotal float not null,
    Pagado float not null,
    IdVenta int not null,
    IdArticulo int not null,
    CostoUnitario float null
);
go

create table DevolucionVenta (
    Id int identity(1,1) primary key not null,
    Fecha date not null,
    Motivo varchar(150) null,
    IdVenta int not null,
    IdCliente int not null,
    IdUsuario int not null,
    TotalDevuelto float not null,
    TotalCambio float not null,
    AplicadoADeudaVenta float not null,
    MontoCobradoAhora float not null,
    PorPagar float not null,
    MontoDevueltoEfectivo float not null,
    SaldoAFavorGenerado float not null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null
);
go

create table EntregaBien (
    Id int identity(1,1) primary key not null,
    Fecha date not null,
    NumeroBoleta varchar(30) null,
    Cantidad float not null,
    PrecioUnitario float null,
    SubTotal float null,
    Nota varchar(150) null,
    IdCliente int not null,
    IdUsuario int not null,
    IdTipoBien int not null,
    IdLiquidacion int null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null
);
go

create table Familia (
    Id int identity(1,1) primary key not null,
    NombreFamilia varchar(50) not null,
    Descripcion varchar(150) null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null
);
go

create table FormaDePago (
    Id int identity(1,1) primary key not null,
    FormaPago varchar(30) not null,
    EsEfectivo bit default ((0)) not null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null
);
go

create table Liquidacion (
    Id int identity(1,1) primary key not null,
    Fecha date not null,
    Nota varchar(150) null,
    IdCliente int not null,
    IdUsuario int not null,
    DeudaAntes float not null,
    TotalEntregado float not null,
    DeudaActual float not null,
    MontoDevueltoEfectivo float not null,
    SaldoAFavorGenerado float not null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null
);
go

create table MovimientoCaja (
    Id int identity(1,1) primary key not null,
    Fecha date not null,
    Tipo varchar(10) not null,
    Monto float not null,
    Motivo varchar(200) not null,
    IdUsuario int not null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null
);
go

create table AjusteStock (
    Id int identity(1,1) primary key not null,
    Fecha date not null,
    Tipo varchar(10) not null,
    Cantidad int not null,
    Motivo varchar(200) not null,
    IdArticulo int not null,
    IdUsuario int not null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null
);
go

create table Pago (
    Id int identity(1,1) primary key not null,
    DeudaActual float not null,
    Fecha date not null,
    Monto float not null,
    Recibo varchar(30) null,
    Nota varchar(150) null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null,
    IdProveedor int not null,
    IdUsuario int not null
);
go

create table Promocion (
    Id int identity(1,1) primary key not null,
    NombrePromocion varchar(80) not null,
    DescuentoMonetario money null,
    DescuentoProcentaje float null,
    Descripcion varchar(150) null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null
);
go

create table Proveedor (
    Id int identity(1,1) primary key not null,
    Nombre varchar(50) not null,
    Nit varchar(30) null,
    PersonaContacto varchar(50) null,
    Direccion varchar(100) null,
    Zona varchar(100) null,
    Telefono int null,
    Correo varchar(50) null,
    Nota varchar(100) null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaje datetime null
);
go

create table Temporada (
    Id int identity(1,1) primary key not null,
    Nombre varchar(50) not null,
    MesInicio int not null,
    MesFin int not null,
    Estado char(2) default ('AC') not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null
);
go

create table TipoBien (
    Id int identity(1,1) primary key not null,
    Nombre varchar(50) not null,
    UnidadMedida varchar(30) not null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null
);
go

create table Usuario (
    Id int identity(1,1) primary key not null,
    Nombre varchar(50) not null,
    DocumentoIdentidad varchar(20) not null,
    NIT varchar(50) null,
    Direccion varchar(100) null,
    Zona varchar(100) null,
    Telefono int null,
    Correo varchar(30) null,
    Nota varchar(200) null,
    NombreUSuario varchar(50) not null,
    Contrasena varchar(100) not null,
    ConfirmarContrasena varchar(100) not null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActilizado datetime null,
    FechaBaja datetime null,
    EsAdministrador bit default ((0)) not null,
    Permisos varchar(500) default ('') not null
);
go

create table Venta (
    Id int identity(1,1) primary key not null,
    Fecha date not null,
    Referencias varchar(100) null,
    DescuentoMonetario float null,
    DescuentoPorcentaje float null,
    Total float not null,
    Pagado float not null,
    PorPagar float not null,
    Nota varchar(150) null,
    Estado char(2) not null,
    UserRegistro varchar(30) null,
    UserActualizado varchar(30) null,
    UserBaja varchar(30) null,
    FechaRegistro datetime null,
    FechaActualizado datetime null,
    FechaBaja datetime null,
    IdCliente int not null,
    IdUsuario int not null,
    IdPromocion int null,
    MontoSaldoAFavorAplicado float default ((0)) not null,
    IdFormaDePago int null
);
go

-- ============================================================================
-- Foreign keys (se agregan al final para no depender del orden de creacion)
-- ============================================================================

alter table Articulo add constraint FK__Articulo__IdFami__1DE57479 foreign key (IdFamilia) references Familia (Id) on delete cascade on update cascade;
go
alter table Articulo add constraint FK__Articulo__IdProm__1ED998B2 foreign key (IdPromocion) references Promocion (Id) on delete set null;
go
alter table ArticuloCaracteristica add constraint FK__ArticuloC__IdArt__22AA2996 foreign key (IdArticulo) references Articulo (Id) on delete cascade;
go
alter table ArticuloCaracteristica add constraint FK__ArticuloC__IdCar__239E4DCF foreign key (IdCaracteristica) references Caracteristica (Id);
go
alter table Cobro add constraint FK__Cobro__IdCliente__267ABA7A foreign key (IdCliente) references Cliente (Id) on delete cascade on update cascade;
go
alter table Cobro add constraint FK__Cobro__IdUsuario__276EDEB3 foreign key (IdUsuario) references Usuario (Id) on delete cascade on update cascade;
go
alter table Compra add constraint FK__Compra__IdProvee__2F10007B foreign key (IdProveedor) references Proveedor (Id) on delete cascade on update cascade;
go
alter table DetalleCompra add constraint FK__DetalleCo__IdArt__32E0915F foreign key (IdArticulo) references Articulo (Id);
go
alter table DetalleCompra add constraint FK__DetalleCo__IdCom__31EC6D26 foreign key (IdCompra) references Compra (Id) on delete cascade on update cascade;
go
alter table DetalleVenta add constraint FK__DetalleVe__IdArt__3B75D760 foreign key (IdArticulo) references Articulo (Id);
go
alter table DetalleVenta add constraint FK__DetalleVe__IdVen__3A81B327 foreign key (IdVenta) references Venta (Id) on delete cascade on update cascade;
go
alter table Pago add constraint FK__Pago__IdProveedo__2A4B4B5E foreign key (IdProveedor) references Proveedor (Id) on delete cascade on update cascade;
go
alter table Pago add constraint FK__Pago__IdUsuario__2B3F6F97 foreign key (IdUsuario) references Usuario (Id) on delete cascade on update cascade;
go
alter table Venta add constraint FK__Venta__IdCliente__35BCFE0A foreign key (IdCliente) references Cliente (Id) on delete cascade on update cascade;
go
alter table Venta add constraint FK__Venta__IdPromoci__37A5467C foreign key (IdPromocion) references Promocion (Id) on delete set null;
go
alter table Venta add constraint FK__Venta__IdUsuario__36B12243 foreign key (IdUsuario) references Usuario (Id) on delete cascade on update cascade;
go
alter table Venta add constraint FK_Venta_FormaDePago foreign key (IdFormaDePago) references FormaDePago (Id) on delete set null;
go
alter table Compra add constraint FK_Compra_Usuario foreign key (IdUsuario) references Usuario (Id);
go
alter table ConfiguracionEmpresa add constraint FK_ConfiguracionEmpresa_Cliente_IdClientePorDefecto foreign key (IdClientePorDefecto) references Cliente (Id) on delete set null;
go
alter table ConfiguracionEmpresa add constraint FK_ConfiguracionEmpresa_Proveedor_IdProveedorPorDefecto foreign key (IdProveedorPorDefecto) references Proveedor (Id) on delete set null;
go
alter table DetalleCambioVenta add constraint FK_DetalleCambioVenta_Articulo foreign key (IdArticulo) references Articulo (Id);
go
alter table DetalleCambioVenta add constraint FK_DetalleCambioVenta_Devolucion foreign key (IdDevolucion) references DevolucionVenta (Id) on delete cascade;
go
alter table DetalleDevolucionVenta add constraint FK_DetalleDevolucionVenta_Articulo foreign key (IdArticulo) references Articulo (Id);
go
alter table DetalleDevolucionVenta add constraint FK_DetalleDevolucionVenta_DetalleVenta foreign key (IdDetalleVenta) references DetalleVenta (Id);
go
alter table DetalleDevolucionVenta add constraint FK_DetalleDevolucionVenta_Devolucion foreign key (IdDevolucion) references DevolucionVenta (Id) on delete cascade;
go
alter table DevolucionVenta add constraint FK_DevolucionVenta_Cliente foreign key (IdCliente) references Cliente (Id);
go
alter table DevolucionVenta add constraint FK_DevolucionVenta_Usuario foreign key (IdUsuario) references Usuario (Id);
go
alter table DevolucionVenta add constraint FK_DevolucionVenta_Venta foreign key (IdVenta) references Venta (Id);
go
alter table EntregaBien add constraint FK_EntregaBien_Cliente foreign key (IdCliente) references Cliente (Id);
go
alter table EntregaBien add constraint FK_EntregaBien_Liquidacion foreign key (IdLiquidacion) references Liquidacion (Id);
go
alter table EntregaBien add constraint FK_EntregaBien_TipoBien foreign key (IdTipoBien) references TipoBien (Id);
go
alter table EntregaBien add constraint FK_EntregaBien_Usuario foreign key (IdUsuario) references Usuario (Id);
go
alter table Liquidacion add constraint FK_Liquidacion_Cliente foreign key (IdCliente) references Cliente (Id);
go
alter table Liquidacion add constraint FK_Liquidacion_Usuario foreign key (IdUsuario) references Usuario (Id);
go
alter table MovimientoCaja add constraint FK_MovimientoCaja_Usuario foreign key (IdUsuario) references Usuario (Id);
go
alter table AjusteStock add constraint FK_AjusteStock_Articulo foreign key (IdArticulo) references Articulo (Id);
go
alter table AjusteStock add constraint FK_AjusteStock_Usuario foreign key (IdUsuario) references Usuario (Id);
go

-- No es SQL, es un comando de Package Manager Console (Visual Studio) para
-- generar el DbContext por scaffolding si alguna vez lo necesitas. No corre en SSMS:
-- Scaffold-DBContext "Data Source=localhost\SQLEXPRESS;Initial Catalog=VentSoft;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model
