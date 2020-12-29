namespace BCKFreightTMS.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InitialModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsernameChangeLimit",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ActionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    StreetLine = table.Column<string>(maxLength: 100, nullable: true),
                    Postcode = table.Column<string>(maxLength: 20, nullable: true),
                    City = table.Column<string>(maxLength: 200, nullable: true),
                    State = table.Column<string>(maxLength: 200, nullable: true),
                    Area = table.Column<string>(maxLength: 200, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CargoTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comunicators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Mobile1 = table.Column<string>(type: "varchar(10)", nullable: false),
                    Mobile2 = table.Column<string>(type: "varchar(10)", nullable: true),
                    Phone1 = table.Column<string>(type: "varchar(10)", nullable: true),
                    Phone2 = table.Column<string>(type: "varchar(10)", nullable: true),
                    Email1 = table.Column<string>(type: "varchar(50)", nullable: true),
                    Email2 = table.Column<string>(type: "varchar(50)", nullable: true),
                    Details = table.Column<string>(type: "text", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comunicators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxCountries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 90, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxCountries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleLoadingBodies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 80, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleLoadingBodies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 80, nullable: false),
                    MyProperty = table.Column<string>(type: "text", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    AddressId = table.Column<int>(nullable: false),
                    MOLFirstName = table.Column<string>(maxLength: 80, nullable: true),
                    MOLLastName = table.Column<string>(maxLength: 80, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyAddresses_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cargos",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Lenght = table.Column<float>(type: "real", nullable: false),
                    Width = table.Column<float>(type: "real", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    WeightGross = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WeightNet = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Cubature = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    MyProperty = table.Column<string>(type: "text", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cargos_CargoTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CargoTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    TaxCountryId = table.Column<int>(nullable: false),
                    TaxNumber = table.Column<string>(type: "varchar(20)", nullable: true),
                    CompanyAddressId = table.Column<int>(nullable: false),
                    ComunicatorsId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_CompanyAddresses_CompanyAddressId",
                        column: x => x.CompanyAddressId,
                        principalTable: "CompanyAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Comunicators_ComunicatorsId",
                        column: x => x.ComunicatorsId,
                        principalTable: "Comunicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_TaxCountries_TaxCountryId",
                        column: x => x.TaxCountryId,
                        principalTable: "TaxCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BankDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<string>(nullable: false),
                    BankName = table.Column<string>(maxLength: 100, nullable: true),
                    BankCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    BankIban = table.Column<string>(type: "varchar(22)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankDetails_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(type: "varchar(10)", nullable: true),
                    AddressId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyContacts_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyContacts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 80, nullable: false),
                    LastName = table.Column<string>(maxLength: 80, nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    ComunicatorsId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_Comunicators_ComunicatorsId",
                        column: x => x.ComunicatorsId,
                        principalTable: "Comunicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_PersonRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "PersonRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderFroms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    ReferenceNum = table.Column<string>(nullable: true),
                    PriceNetIn = table.Column<decimal>(nullable: false),
                    DueDays = table.Column<int>(nullable: false),
                    CompanyId = table.Column<string>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    ContactId = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderFroms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderFroms_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderFroms_People_ContactId",
                        column: x => x.ContactId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderFroms_OrderTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "OrderTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    LoadingBodyId = table.Column<int>(nullable: false),
                    DriverId = table.Column<string>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    TrailerId = table.Column<string>(nullable: true),
                    RegNumber = table.Column<string>(maxLength: 10, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Details = table.Column<string>(type: "text", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_People_DriverId",
                        column: x => x.DriverId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleLoadingBodies_LoadingBodyId",
                        column: x => x.LoadingBodyId,
                        principalTable: "VehicleLoadingBodies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Vehicles_TrailerId",
                        column: x => x.TrailerId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderTos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    ReferenceNum = table.Column<string>(nullable: true),
                    PriceNetOut = table.Column<decimal>(nullable: false),
                    DueDays = table.Column<int>(nullable: false),
                    CompanyId = table.Column<string>(nullable: false),
                    VehicleId = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    ContactId = table.Column<string>(nullable: true),
                    PersonId = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTos_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderTos_People_ContactId",
                        column: x => x.ContactId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderTos_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderTos_OrderTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "OrderTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderTos_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DriverOrders",
                columns: table => new
                {
                    DriverId = table.Column<string>(nullable: false),
                    OrderId = table.Column<string>(nullable: false),
                    OrderId1 = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverOrders", x => new { x.DriverId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_DriverOrders_People_DriverId",
                        column: x => x.DriverId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DriverOrders_OrderTos_OrderId1",
                        column: x => x.OrderId1,
                        principalTable: "OrderTos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatorId = table.Column<string>(nullable: false),
                    CargoId = table.Column<string>(nullable: false),
                    OrderToId = table.Column<int>(nullable: false),
                    OrderFromId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Cargos_CargoId",
                        column: x => x.CargoId,
                        principalTable: "Cargos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderFroms_OrderFromId",
                        column: x => x.OrderFromId,
                        principalTable: "OrderFroms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderTos_OrderToId",
                        column: x => x.OrderToId,
                        principalTable: "OrderTos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderActions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    OrderId = table.Column<string>(nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    Until = table.Column<DateTime>(nullable: false),
                    Details = table.Column<string>(type: "text", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderActions_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderActions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderActions_ActionTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ActionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionTypes_IsDeleted",
                table: "ActionTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_IsDeleted",
                table: "Addresses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_BankDetails_CompanyId",
                table: "BankDetails",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BankDetails_IsDeleted",
                table: "BankDetails",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_IsDeleted",
                table: "Cargos",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_TypeId",
                table: "Cargos",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CargoTypes_IsDeleted",
                table: "CargoTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyAddressId",
                table: "Companies",
                column: "CompanyAddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ComunicatorsId",
                table: "Companies",
                column: "ComunicatorsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_IsDeleted",
                table: "Companies",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_TaxCountryId",
                table: "Companies",
                column: "TaxCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAddresses_AddressId",
                table: "CompanyAddresses",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAddresses_IsDeleted",
                table: "CompanyAddresses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyContacts_AddressId",
                table: "CompanyContacts",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyContacts_CompanyId",
                table: "CompanyContacts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyContacts_IsDeleted",
                table: "CompanyContacts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Comunicators_IsDeleted",
                table: "Comunicators",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DriverOrders_IsDeleted",
                table: "DriverOrders",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DriverOrders_OrderId1",
                table: "DriverOrders",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderActions_AddressId",
                table: "OrderActions",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderActions_IsDeleted",
                table: "OrderActions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OrderActions_OrderId",
                table: "OrderActions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderActions_TypeId",
                table: "OrderActions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFroms_CompanyId",
                table: "OrderFroms",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFroms_ContactId",
                table: "OrderFroms",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFroms_IsDeleted",
                table: "OrderFroms",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFroms_TypeId",
                table: "OrderFroms",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CargoId",
                table: "Orders",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatorId",
                table: "Orders",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IsDeleted",
                table: "Orders",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderFromId",
                table: "Orders",
                column: "OrderFromId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderToId",
                table: "Orders",
                column: "OrderToId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusId",
                table: "Orders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_IsDeleted",
                table: "OrderStatuses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTos_CompanyId",
                table: "OrderTos",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTos_ContactId",
                table: "OrderTos",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTos_IsDeleted",
                table: "OrderTos",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTos_PersonId",
                table: "OrderTos",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTos_TypeId",
                table: "OrderTos",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTos_VehicleId",
                table: "OrderTos",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTypes_IsDeleted",
                table: "OrderTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_People_CompanyId",
                table: "People",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_People_ComunicatorsId",
                table: "People",
                column: "ComunicatorsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_IsDeleted",
                table: "People",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_People_RoleId",
                table: "People",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonRoles_IsDeleted",
                table: "PersonRoles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TaxCountries_IsDeleted",
                table: "TaxCountries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLoadingBodies_IsDeleted",
                table: "VehicleLoadingBodies",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CompanyId",
                table: "Vehicles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DriverId",
                table: "Vehicles",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_IsDeleted",
                table: "Vehicles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_LoadingBodyId",
                table: "Vehicles",
                column: "LoadingBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_TrailerId",
                table: "Vehicles",
                column: "TrailerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_TypeId",
                table: "Vehicles",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypes_IsDeleted",
                table: "VehicleTypes",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankDetails");

            migrationBuilder.DropTable(
                name: "CompanyContacts");

            migrationBuilder.DropTable(
                name: "DriverOrders");

            migrationBuilder.DropTable(
                name: "OrderActions");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ActionTypes");

            migrationBuilder.DropTable(
                name: "Cargos");

            migrationBuilder.DropTable(
                name: "OrderFroms");

            migrationBuilder.DropTable(
                name: "OrderTos");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "CargoTypes");

            migrationBuilder.DropTable(
                name: "OrderTypes");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "VehicleLoadingBodies");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "PersonRoles");

            migrationBuilder.DropTable(
                name: "CompanyAddresses");

            migrationBuilder.DropTable(
                name: "Comunicators");

            migrationBuilder.DropTable(
                name: "TaxCountries");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UsernameChangeLimit",
                table: "AspNetUsers");
        }
    }
}
