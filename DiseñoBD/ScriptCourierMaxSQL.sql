/* ============================================================================
   ----------------------------------------------------------------------------
   CourierMax - Esquema de Base de Datos (SQL Server)
   Prueba Técnica .NET Core - Sistema de gestión de envíos
   ----------------------------------------------------------------------------
   Convención de nombres: tablas con prefijo tbl_.
   ----------------------------------------------------------------------------
============================================================================ */

SET NOCOUNT ON;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE dbo.tbl_Ciudades (
    Id      INT IDENTITY(1,1) CONSTRAINT PK_Ciudades PRIMARY KEY,
    Nombre  NVARCHAR(80) NOT NULL,
    CONSTRAINT UQ_Ciudades_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.tbl_Distancias (
    Id               INT IDENTITY(1,1) CONSTRAINT PK_Distancias PRIMARY KEY,
    CiudadOrigenId   INT NOT NULL,
    CiudadDestinoId  INT NOT NULL,
    DistanciaKm      INT NOT NULL,
    TarifaDistancia  DECIMAL(12,2) NOT NULL,
    CONSTRAINT FK_Distancias_Origen  FOREIGN KEY (CiudadOrigenId)  REFERENCES dbo.tbl_Ciudades(Id),
    CONSTRAINT FK_Distancias_Destino FOREIGN KEY (CiudadDestinoId) REFERENCES dbo.tbl_Ciudades(Id),
    CONSTRAINT UQ_Distancias_Par     UNIQUE (CiudadOrigenId, CiudadDestinoId),
    CONSTRAINT CK_Distancias_NoMismo CHECK (CiudadOrigenId <> CiudadDestinoId),
    CONSTRAINT CK_Distancias_Km      CHECK (DistanciaKm > 0)
);
GO

CREATE TABLE dbo.tbl_Festivos (
    Fecha        DATE NOT NULL CONSTRAINT PK_Festivos PRIMARY KEY,
    Descripcion  NVARCHAR(120) NULL
);
GO

CREATE TABLE dbo.tbl_Rol (
    Id           INT IDENTITY(1,1) CONSTRAINT PK_Rol PRIMARY KEY,
    Codigo       VARCHAR(30)   NOT NULL CONSTRAINT UQ_Rol_Codigo UNIQUE,
    Descripcion  NVARCHAR(120) NOT NULL,
    Orden        INT           NOT NULL CONSTRAINT DF_Rol_Orden DEFAULT (0),
    Activo       BIT           NOT NULL CONSTRAINT DF_Rol_Activo DEFAULT (1)
);
GO

CREATE TABLE dbo.tbl_Usuarios (
    Id      INT IDENTITY(1,1) CONSTRAINT PK_Usuarios PRIMARY KEY,
    Nombre  NVARCHAR(120) NOT NULL,
    RolId   INT NOT NULL,
    CONSTRAINT FK_Usuarios_Rol FOREIGN KEY (RolId) REFERENCES dbo.tbl_Rol(Id)
);
GO

CREATE TABLE dbo.tbl_Vehiculos (
    Id                  INT IDENTITY(1,1) CONSTRAINT PK_Vehiculos PRIMARY KEY,
    Placa               VARCHAR(10) NOT NULL,
    CapacidadPesoKg     DECIMAL(8,2) NOT NULL,
    CapacidadVolumenM3  DECIMAL(8,2) NOT NULL,
    CONSTRAINT UQ_Vehiculos_Placa  UNIQUE (Placa),
    CONSTRAINT CK_Vehiculos_PesoCap CHECK (CapacidadPesoKg > 0),
    CONSTRAINT CK_Vehiculos_VolCap  CHECK (CapacidadVolumenM3 > 0)
);
GO

CREATE TABLE dbo.tbl_Conductores (
    Id          INT IDENTITY(1,1) CONSTRAINT PK_Conductores PRIMARY KEY,
    Nombre      NVARCHAR(120) NOT NULL,
    VehiculoId  INT NOT NULL,
    Activo      BIT NOT NULL CONSTRAINT DF_Conductores_Activo DEFAULT (1),
    CONSTRAINT FK_Conductores_Vehiculo FOREIGN KEY (VehiculoId) REFERENCES dbo.tbl_Vehiculos(Id),
    CONSTRAINT UQ_Conductores_Vehiculo UNIQUE (VehiculoId) 
);
GO

CREATE TABLE dbo.tbl_TipoServicio (
    Id                  INT IDENTITY(1,1) CONSTRAINT PK_TipoServicio PRIMARY KEY,
    Codigo              VARCHAR(30)   NOT NULL CONSTRAINT UQ_TipoServicio_Codigo UNIQUE,
    Descripcion         NVARCHAR(120) NOT NULL,
    TarifaBase          DECIMAL(12,2) NOT NULL,
    DiasSla             INT           NULL,
    Orden               INT           NOT NULL CONSTRAINT DF_TipoServicio_Orden DEFAULT (0),
    Activo              BIT           NOT NULL CONSTRAINT DF_TipoServicio_Activo DEFAULT (1)
);
GO

CREATE TABLE dbo.tbl_TipoPaquete (
    Id           INT IDENTITY(1,1) CONSTRAINT PK_TipoPaquete PRIMARY KEY,
    Codigo       VARCHAR(30)   NOT NULL CONSTRAINT UQ_TipoPaquete_Codigo UNIQUE,
    Descripcion  NVARCHAR(120) NOT NULL,
    RecargoPorcentaje DECIMAL(5,2) NOT NULL CONSTRAINT DF_TipoPaquete_Recargo DEFAULT (0),
    Orden        INT           NOT NULL CONSTRAINT DF_TipoPaquete_Orden DEFAULT (0),
    Activo       BIT           NOT NULL CONSTRAINT DF_TipoPaquete_Activo DEFAULT (1)
);
GO

CREATE TABLE dbo.tbl_ParametroTarifa (
    Id                    INT IDENTITY(1,1) CONSTRAINT PK_ParametroTarifa PRIMARY KEY,
    PesoBaseKg            DECIMAL(6,2)  NOT NULL,
    RecargoPorKgAdicional DECIMAL(12,2) NOT NULL
);
GO

CREATE TABLE dbo.tbl_Envios (
    Id                          INT IDENTITY(1,1) CONSTRAINT PK_Envios PRIMARY KEY,
    CodigoRastreo               VARCHAR(11) NOT NULL,
    Estado                      VARCHAR(15) NOT NULL CONSTRAINT DF_Envios_Estado DEFAULT ('CREADO'),
    TipoServicioId              INT NOT NULL,
    RemitenteNombre             NVARCHAR(120) NOT NULL,
    RemitenteTelefono           VARCHAR(10)   NOT NULL,
    RemitenteDireccionRecogida  NVARCHAR(250) NOT NULL,
    DestinatarioNombre          NVARCHAR(120) NOT NULL,
    DestinatarioTelefono        VARCHAR(10)   NOT NULL,
    DestinatarioDireccionEntrega NVARCHAR(250) NOT NULL,
    PesoKg                      DECIMAL(6,2) NOT NULL,
    LargoCm                     DECIMAL(6,2) NOT NULL,
    AnchoCm                     DECIMAL(6,2) NOT NULL,
    AltoCm                      DECIMAL(6,2) NOT NULL,
    TipoPaqueteId               INT NOT NULL,
    VolumenM3                   AS (LargoCm * AnchoCm * AltoCm / 1000000.0) PERSISTED,
    CiudadOrigenId              INT NOT NULL,
    CiudadDestinoId             INT NOT NULL,
    ConductorId                 INT NULL,
    CostoTotal                  DECIMAL(12,2) NULL,
    FechaCreacion               DATETIME2 NOT NULL CONSTRAINT DF_Envios_FechaCreacion DEFAULT (SYSUTCDATETIME()),
    FechaAsignacion             DATETIME2 NULL,
    FechaEntrega                DATETIME2 NULL,
    CONSTRAINT FK_Envios_CiudadOrigen  FOREIGN KEY (CiudadOrigenId)  REFERENCES dbo.tbl_Ciudades(Id),
    CONSTRAINT FK_Envios_CiudadDestino FOREIGN KEY (CiudadDestinoId) REFERENCES dbo.tbl_Ciudades(Id),
    CONSTRAINT FK_Envios_Conductor     FOREIGN KEY (ConductorId)     REFERENCES dbo.tbl_Conductores(Id),
    CONSTRAINT FK_Envios_TipoServicio  FOREIGN KEY (TipoServicioId)  REFERENCES dbo.tbl_TipoServicio(Id),
    CONSTRAINT FK_Envios_TipoPaquete   FOREIGN KEY (TipoPaqueteId)   REFERENCES dbo.tbl_TipoPaquete(Id),
    CONSTRAINT UQ_Envios_Codigo   UNIQUE (CodigoRastreo),
    CONSTRAINT CK_Envios_Codigo   CHECK (CodigoRastreo LIKE 'CM-[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    CONSTRAINT CK_Envios_Estado   CHECK (Estado IN ('CREADO','ASIGNADO','EN_TRANSITO','ENTREGADO','CANCELADO')),
    CONSTRAINT CK_Envios_TelRem   CHECK (RemitenteTelefono    LIKE '[36][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    CONSTRAINT CK_Envios_TelDest  CHECK (DestinatarioTelefono LIKE '[36][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    CONSTRAINT CK_Envios_Peso     CHECK (PesoKg >= 0.1 AND PesoKg <= 100),
    CONSTRAINT CK_Envios_Largo    CHECK (LargoCm >= 1 AND LargoCm <= 200),
    CONSTRAINT CK_Envios_Ancho    CHECK (AnchoCm >= 1 AND AnchoCm <= 200),
    CONSTRAINT CK_Envios_Alto     CHECK (AltoCm  >= 1 AND AltoCm  <= 200),
    CONSTRAINT CK_Envios_DirRem   CHECK (LEN(LTRIM(RTRIM(RemitenteDireccionRecogida)))  > 0),
    CONSTRAINT CK_Envios_DirDest  CHECK (LEN(LTRIM(RTRIM(DestinatarioDireccionEntrega))) > 0),
    CONSTRAINT CK_Envios_Ruta     CHECK (CiudadOrigenId <> CiudadDestinoId),
    CONSTRAINT CK_Envios_AsignConductor CHECK (
        (Estado IN ('ASIGNADO','EN_TRANSITO','ENTREGADO') AND ConductorId IS NOT NULL)
        OR (Estado IN ('CREADO','CANCELADO'))
    )
);
GO

CREATE TABLE dbo.tbl_HistorialEstados (
    Id                INT IDENTITY(1,1) CONSTRAINT PK_HistorialEstados PRIMARY KEY,
    EnvioId           INT NOT NULL,
    EstadoAnterior    VARCHAR(15) NULL,            
    EstadoNuevo       VARCHAR(15) NOT NULL,
    FechaCambio       DATETIME2 NOT NULL CONSTRAINT DF_Hist_Fecha DEFAULT (SYSUTCDATETIME()),
    Motivo            NVARCHAR(250) NULL,
    RealizadoPorId    INT NULL,                    
    CONSTRAINT FK_Hist_Envio        FOREIGN KEY (EnvioId)        REFERENCES dbo.tbl_Envios(Id),
    CONSTRAINT FK_Hist_Usuario      FOREIGN KEY (RealizadoPorId) REFERENCES dbo.tbl_Usuarios(Id),
    CONSTRAINT CK_Hist_EstadoAnt    CHECK (EstadoAnterior IS NULL OR EstadoAnterior IN ('CREADO','ASIGNADO','EN_TRANSITO','ENTREGADO','CANCELADO')),
    CONSTRAINT CK_Hist_EstadoNue    CHECK (EstadoNuevo IN ('CREADO','ASIGNADO','EN_TRANSITO','ENTREGADO','CANCELADO')),
    CONSTRAINT CK_Hist_MotivoCancel CHECK (
        EstadoNuevo <> 'CANCELADO'
        OR (Motivo IS NOT NULL AND LEN(LTRIM(RTRIM(Motivo))) >= 5)
    )
);
GO

CREATE INDEX IX_Envios_Estado        ON dbo.tbl_Envios(Estado);
CREATE INDEX IX_Envios_Conductor     ON dbo.tbl_Envios(ConductorId) WHERE ConductorId IS NOT NULL;
CREATE INDEX IX_Envios_FechaCreacion ON dbo.tbl_Envios(FechaCreacion);
CREATE INDEX IX_Hist_Envio           ON dbo.tbl_HistorialEstados(EnvioId);
GO

/* ====================== INSERTS ============================== */

INSERT INTO dbo.tbl_Ciudades (Nombre) VALUES
    (N'Bogotá'), (N'Medellín'), (N'Cali'), (N'Barranquilla');
GO

;WITH pares AS (
    SELECT N'Bogotá'   AS O, N'Medellín'     AS D, 480 AS Km, 12000.00 AS Tarifa
    UNION ALL SELECT N'Bogotá',   N'Cali',          360,  9000.00
    UNION ALL SELECT N'Bogotá',   N'Barranquilla',  950, 20000.00
    UNION ALL SELECT N'Medellín', N'Cali',          310,  8000.00
    UNION ALL SELECT N'Medellín', N'Barranquilla',  650, 15000.00
    UNION ALL SELECT N'Cali',     N'Barranquilla',  900, 18000.00
)
INSERT INTO dbo.tbl_Distancias (CiudadOrigenId, CiudadDestinoId, DistanciaKm, TarifaDistancia)
SELECT co.Id, cd.Id, p.Km, p.Tarifa
FROM pares p
JOIN dbo.tbl_Ciudades co ON co.Nombre = p.O
JOIN dbo.tbl_Ciudades cd ON cd.Nombre = p.D
UNION ALL  -- sentido inverso
SELECT cd.Id, co.Id, p.Km, p.Tarifa
FROM pares p
JOIN dbo.tbl_Ciudades co ON co.Nombre = p.O
JOIN dbo.tbl_Ciudades cd ON cd.Nombre = p.D;
GO

INSERT INTO dbo.tbl_Vehiculos (Placa, CapacidadPesoKg, CapacidadVolumenM3) VALUES
    ('ABC123', 500, 10),
    ('DEF456', 300,  6),
    ('GHI789', 800, 15);
GO

INSERT INTO dbo.tbl_Conductores (Nombre, VehiculoId, Activo)
SELECT N'Juan Pérez',  Id, 1 FROM dbo.tbl_Vehiculos WHERE Placa = 'ABC123'
UNION ALL SELECT N'María López', Id, 1 FROM dbo.tbl_Vehiculos WHERE Placa = 'DEF456'
UNION ALL SELECT N'Carlos Ruiz', Id, 1 FROM dbo.tbl_Vehiculos WHERE Placa = 'GHI789';
GO

INSERT INTO dbo.tbl_Rol (Codigo, Descripcion, Orden) VALUES
    ('ADMIN',     N'Administrador', 1),
    ('OPERADOR',  N'Operador',      2),
    ('CONDUCTOR', N'Conductor',     3),
    ('SISTEMA',   N'Sistema',       4);
GO

INSERT INTO dbo.tbl_Usuarios (Nombre, RolId)
SELECT N'Sistema',    Id FROM dbo.tbl_Rol WHERE Codigo = 'SISTEMA'
UNION ALL SELECT N'Operador 1', Id FROM dbo.tbl_Rol WHERE Codigo = 'OPERADOR';
GO

INSERT INTO dbo.tbl_TipoServicio (Codigo, Descripcion, TarifaBase, DiasSla, Orden) VALUES
    ('ESTANDAR',  N'Servicio estándar',  8000, 5, 1),
    ('EXPRESS',   N'Servicio express',  15000, 2, 2),
    ('MISMO_DIA', N'Entrega mismo día', 25000, 1, 3);
GO

INSERT INTO dbo.tbl_TipoPaquete (Codigo, Descripcion, RecargoPorcentaje, Orden) VALUES
    ('DOCUMENTO',  N'Documento',   0, 1),
    ('PAQUETE',    N'Paquete',     0, 2),
    ('FRAGIL',     N'Frágil',     30, 3),
    ('PERECEDERO', N'Perecedero', 25, 4);
GO

INSERT INTO dbo.tbl_ParametroTarifa (PesoBaseKg, RecargoPorKgAdicional) VALUES
    (2, 1500);
GO

INSERT INTO dbo.tbl_Festivos (Fecha, Descripcion) VALUES
    ('2026-01-01', N'Año Nuevo'),
    ('2026-01-26', N'Reyes Magos (traslado)'),
    ('2026-01-30', N'Festivo'),
    ('2026-03-24', N'Festivo'),
    ('2026-05-01', N'Día del Trabajo'),
    ('2026-06-01', N'Festivo'),
    ('2026-06-29', N'San Pedro y San Pablo'),
    ('2026-07-20', N'Independencia'),
    ('2026-08-17', N'Batalla de Boyacá (traslado)'),
    ('2026-10-20', N'Festivo'),
    ('2026-11-09', N'Festivo'),
    ('2026-12-08', N'Inmaculada Concepción');
GO

PRINT 'CourierMax: esquema y datos de referencia creados correctamente.';
GO
