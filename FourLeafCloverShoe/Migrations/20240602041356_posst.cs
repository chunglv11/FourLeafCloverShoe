using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FourLeafCloverShoe.Migrations
{
    public partial class posst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("02a68c66-dd8f-4d01-9c64-58ce304dada4"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("171354c5-2603-44e1-b521-e61ecd474dfc"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("4511e450-386e-4d74-adcb-0cf13ca38a98"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("47689bfd-ff93-4f62-a9a9-1c6ee77af5b5"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("6d581b68-83ee-4318-a393-5622bef44647"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("6e121b13-aed7-4842-90de-17ffcf8707d0"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("88ed3da5-2633-4af3-bb73-5f2cd0744891"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("c7b9e883-c041-43bd-8dd7-c1327cdcd183"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("d3aabb78-2bea-423f-b8fb-949c53ffc8ca"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("d4e71dde-320b-45ca-b742-9fe6786b88ac"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("1376ed5e-ea37-49e2-92ee-0f99859db8ef"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("1e531175-b390-440c-92b6-611d70028b38"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("451387cd-0c22-4bd6-84b5-fdc7468f8525"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("71d134ff-13e3-493c-94d3-652786ce0f7a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ab54a60d-de70-48d9-882f-4e701ec0c82e"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b7eac232-1c5f-4453-93ef-61688d84bfc1"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e4709a2b-db76-42f7-8514-5f87f25cf6d2"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fa90e4ee-d6c9-40b4-8879-fb68a20ee9c5"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("0855abe1-edb1-4516-a9dd-915ce516b0ed"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("14f3bb86-6101-4947-953d-582ca0a16754"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("226f72be-5aca-4765-a980-4327a1c64c84"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("40da0a69-2d59-46db-9576-02bdd6b40f88"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("6040f820-2b81-4dcf-afa7-34263e5dc25a"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("67ea400c-4b47-4c1e-a94f-dabed54ba065"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("7350da43-0803-43b0-9b89-3180edb5c52c"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("8de3048d-d7b9-4fb3-ac4d-555e89d11fb1"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("95e0c864-0dce-4c76-9a43-3eb7118a7f2d"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("a02748aa-763c-418d-9421-5d72fb59b9ba"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("a254ad77-02ad-4dd3-a70c-3375673dd3d7"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("c01c054d-117c-4947-b327-fbb148a6b965"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("df3c5739-7497-4388-b4bc-2e0ee8fdfc73"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("f70d15f6-2db5-48e3-856f-2851bc783484"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("f8aa9493-dd4a-44a0-8b30-9c275942a0d5"));

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: new Guid("15540aa1-9865-4186-9436-e43c8b007a98"));

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: new Guid("8d6648af-e40d-417e-91e1-be5472d88ea3"));

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: new Guid("c0a20e21-3240-4b29-bf75-c99ecfb726c6"));

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: new Guid("873f822d-d1e1-49ad-bc5a-42904cf66d14"));

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: new Guid("b9747598-ed96-48d3-87a3-eef9ea73159d"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("4276f0ea-bac1-4047-8d0e-1323d92a874e"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("51252a21-8932-4555-bf7f-ffcec328c3a7"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("58b51d20-7d28-451e-9645-615e04278c95"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("753c9050-7664-4b9f-8701-1ec35d4d6262"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("aab408c2-d60f-4bf6-9648-e1758905a642"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("bca09b33-fee6-4859-b950-b859f2294f12"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("e7f96604-8728-45e1-b166-acd2d517d90a"));

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1ab74dac-9370-42bb-a28a-ce242cd7ab19"), "Thượng Đình" },
                    { new Guid("3896300d-183a-43fe-909b-d5249374d35a"), "MWC" },
                    { new Guid("3b32eb8c-c651-4674-97bf-aa613f36a66e"), "Biti's" },
                    { new Guid("41fe3b03-c103-44ed-88a9-d31ba453dfd6"), "Nike" },
                    { new Guid("67fac991-8738-40f0-87ea-c61e718a38e1"), "Vascara" },
                    { new Guid("74e24256-04a1-4ffa-8b07-5eab4be65ca7"), "Laforce" },
                    { new Guid("7e0e4606-d687-4d30-a294-f6b2542fd441"), "Đông Hải" },
                    { new Guid("861e61dc-b90f-4bd6-bfd8-0c8788a40c34"), "Ananas" },
                    { new Guid("ccb93843-5c68-47d2-bd06-e0edbab835d4"), "Juno" },
                    { new Guid("ed1dcf71-20fe-43a0-bb0b-e401c119519a"), "Adidas" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0e0c4da8-c147-4752-9e3b-694de1e68130"), "Giày thể thao" },
                    { new Guid("175e21d3-f30a-495b-83ca-1c000a528eba"), "Giày cao gót" },
                    { new Guid("24e08766-b0a0-43a0-810f-c790843c2806"), "Dép" },
                    { new Guid("5b394e24-55d3-47d1-8a81-c76f9a43be46"), "Giày sandal" },
                    { new Guid("621bc616-5477-4195-a152-b73a46e50055"), "Sneakers" },
                    { new Guid("69a5d1c6-da96-425d-9ce1-8bf42bad14ab"), "Giày lười" },
                    { new Guid("9b454b3d-2063-4ec1-a816-72b2ed3683f0"), "Giày da" },
                    { new Guid("d63fbc47-c0de-4eaf-a411-6054ed936101"), "Giày boot" }
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "ColorCode", "ColorName" },
                values: new object[,]
                {
                    { new Guid("2beb15f0-fcba-49dd-87a9-06e3ca9b5a47"), "#FFDAB9", "Hồng đào" },
                    { new Guid("46d0ba6f-49d2-4889-9d15-bbe8a1a47508"), "#40E0D0", "Ngọc lam" },
                    { new Guid("59d8886b-1b5f-4495-a688-50ef7270357a"), "#800080", "Tím" },
                    { new Guid("5d22b18f-0252-4f15-873d-69bd4a3839bc"), "#FF0000", "Đỏ" },
                    { new Guid("7c0d2180-d34d-40c3-aa40-463468cf918f"), "#E6E6FA", "Tím hoa cà" },
                    { new Guid("7cfb2f5e-aba3-42c4-b3d6-c401be31c8c6"), "#A52A2A", "Nâu" },
                    { new Guid("81bbe782-34ad-4bda-8547-7810d5ce52dd"), "#008000", "Lục" },
                    { new Guid("ca2026ed-fbd2-42df-ac7c-529e3b4e46a8"), "#FFC0CB", "Hồng" },
                    { new Guid("d8e06285-c1b3-44d1-a942-024660f95cfe"), "#0000FF", "Lam" },
                    { new Guid("da63e901-f9cc-4919-bafd-2d2804c4fdb6"), "#FFA500", "Cam" },
                    { new Guid("f1b56ca8-c850-4a7b-98b3-6374583d66f0"), "#808080", "Xám" },
                    { new Guid("f6b9dfe3-13a1-4c94-917a-b771463b30b5"), "#000000", "Đen" },
                    { new Guid("ff767e8e-71c3-497b-a7d8-6f978cb47d57"), "#FFFF00", "Vàng" },
                    { new Guid("ff892448-f8e0-442f-9ac5-789250e48898"), "#FFFFFF", "Trắng" },
                    { new Guid("fffa3028-6341-4db6-bfe1-040989c98206"), "#4B0082", "Chàm" }
                });

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "Id", "Name", "Status" },
                values: new object[,]
                {
                    { new Guid("1580acb7-9aa2-425a-abe7-e39df811648b"), "momo", true },
                    { new Guid("19afe27d-56c3-4685-90d3-9180e6f0f241"), "vnpay", true },
                    { new Guid("3c711591-ebb1-4bee-b647-b1106729b923"), "cod", true }
                });

            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "Id", "Description", "Name", "PoinsMax", "PointsMin" },
                values: new object[,]
                {
                    { new Guid("8796faaa-0617-4511-8779-9c34ad9b81c9"), null, "Kim Cương", 10000000, 3000001 },
                    { new Guid("ecdc3b10-0dc9-4be9-b208-7c310a60b64c"), null, "Vàng", 3000000, 1000001 }
                });

            migrationBuilder.UpdateData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2FA6148D-B530-421F-878E-CE1D54BFC6AB",
                column: "ConcurrencyStamp",
                value: "bd3c0e01-994f-425c-92a3-b38581b3c12f");

            migrationBuilder.UpdateData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2FA6148D-B530-421F-878E-CE2D54BFC6AB",
                column: "ConcurrencyStamp",
                value: "bbb77f06-5b86-43be-ab9c-b7733d90bb32");

            migrationBuilder.UpdateData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2FA6148D-B530-421F-878E-CE4D54BFC6AB",
                column: "ConcurrencyStamp",
                value: "87844216-68f5-40ee-9632-3ac37230b7a6");

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("550e863c-9d6d-426d-a435-46dad4eba0d1"), "43" });

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("83004364-4cd3-4472-9290-40f469e2f168"), "40" },
                    { new Guid("b36b6711-5b52-4852-9f1e-5f3d6c6ae3e8"), "42" },
                    { new Guid("bc00a134-a35e-4518-bfef-57790c895dc5"), "39" },
                    { new Guid("cf30c96a-cc6b-4e23-8ac5-6152ee6ef7f7"), "44" },
                    { new Guid("e870b6fe-a326-4320-b097-d7a4845b7bff"), "41" },
                    { new Guid("fb26f8b6-3ded-4327-836b-9831a559d4e3"), "38" }
                });

            migrationBuilder.UpdateData(
                schema: "security",
                table: "Users",
                keyColumn: "Id",
                keyValue: "1FA6148D-B530-421F-878E-CE4D54BFC6AB",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "804d453e-042b-47c1-b057-0ca61e4bdb3a", "03ac67cf-0115-430a-8519-6cfcece2759e" });

            migrationBuilder.UpdateData(
                schema: "security",
                table: "Users",
                keyColumn: "Id",
                keyValue: "2FA6148D-B530-421F-878E-CE4D54BFC6AB",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a5e60011-d9e0-4dbb-939a-bc0034bd6d4f", "bc88b3c9-babe-4954-8d64-4556f71e54ea" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("1ab74dac-9370-42bb-a28a-ce242cd7ab19"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("3896300d-183a-43fe-909b-d5249374d35a"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("3b32eb8c-c651-4674-97bf-aa613f36a66e"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("41fe3b03-c103-44ed-88a9-d31ba453dfd6"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("67fac991-8738-40f0-87ea-c61e718a38e1"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("74e24256-04a1-4ffa-8b07-5eab4be65ca7"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("7e0e4606-d687-4d30-a294-f6b2542fd441"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("861e61dc-b90f-4bd6-bfd8-0c8788a40c34"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("ccb93843-5c68-47d2-bd06-e0edbab835d4"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("ed1dcf71-20fe-43a0-bb0b-e401c119519a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0e0c4da8-c147-4752-9e3b-694de1e68130"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("175e21d3-f30a-495b-83ca-1c000a528eba"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("24e08766-b0a0-43a0-810f-c790843c2806"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5b394e24-55d3-47d1-8a81-c76f9a43be46"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("621bc616-5477-4195-a152-b73a46e50055"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("69a5d1c6-da96-425d-9ce1-8bf42bad14ab"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9b454b3d-2063-4ec1-a816-72b2ed3683f0"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d63fbc47-c0de-4eaf-a411-6054ed936101"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("2beb15f0-fcba-49dd-87a9-06e3ca9b5a47"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("46d0ba6f-49d2-4889-9d15-bbe8a1a47508"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("59d8886b-1b5f-4495-a688-50ef7270357a"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("5d22b18f-0252-4f15-873d-69bd4a3839bc"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("7c0d2180-d34d-40c3-aa40-463468cf918f"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("7cfb2f5e-aba3-42c4-b3d6-c401be31c8c6"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("81bbe782-34ad-4bda-8547-7810d5ce52dd"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("ca2026ed-fbd2-42df-ac7c-529e3b4e46a8"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("d8e06285-c1b3-44d1-a942-024660f95cfe"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("da63e901-f9cc-4919-bafd-2d2804c4fdb6"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("f1b56ca8-c850-4a7b-98b3-6374583d66f0"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("f6b9dfe3-13a1-4c94-917a-b771463b30b5"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("ff767e8e-71c3-497b-a7d8-6f978cb47d57"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("ff892448-f8e0-442f-9ac5-789250e48898"));

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: new Guid("fffa3028-6341-4db6-bfe1-040989c98206"));

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: new Guid("1580acb7-9aa2-425a-abe7-e39df811648b"));

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: new Guid("19afe27d-56c3-4685-90d3-9180e6f0f241"));

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: new Guid("3c711591-ebb1-4bee-b647-b1106729b923"));

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: new Guid("8796faaa-0617-4511-8779-9c34ad9b81c9"));

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: new Guid("ecdc3b10-0dc9-4be9-b208-7c310a60b64c"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("550e863c-9d6d-426d-a435-46dad4eba0d1"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("83004364-4cd3-4472-9290-40f469e2f168"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("b36b6711-5b52-4852-9f1e-5f3d6c6ae3e8"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("bc00a134-a35e-4518-bfef-57790c895dc5"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("cf30c96a-cc6b-4e23-8ac5-6152ee6ef7f7"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("e870b6fe-a326-4320-b097-d7a4845b7bff"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("fb26f8b6-3ded-4327-836b-9831a559d4e3"));

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("02a68c66-dd8f-4d01-9c64-58ce304dada4"), "Juno" },
                    { new Guid("171354c5-2603-44e1-b521-e61ecd474dfc"), "Ananas" },
                    { new Guid("4511e450-386e-4d74-adcb-0cf13ca38a98"), "Nike" },
                    { new Guid("47689bfd-ff93-4f62-a9a9-1c6ee77af5b5"), "Laforce" },
                    { new Guid("6d581b68-83ee-4318-a393-5622bef44647"), "MWC" },
                    { new Guid("6e121b13-aed7-4842-90de-17ffcf8707d0"), "Adidas" },
                    { new Guid("88ed3da5-2633-4af3-bb73-5f2cd0744891"), "Thượng Đình" },
                    { new Guid("c7b9e883-c041-43bd-8dd7-c1327cdcd183"), "Biti's" },
                    { new Guid("d3aabb78-2bea-423f-b8fb-949c53ffc8ca"), "Đông Hải" },
                    { new Guid("d4e71dde-320b-45ca-b742-9fe6786b88ac"), "Vascara" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1376ed5e-ea37-49e2-92ee-0f99859db8ef"), "Sneakers" },
                    { new Guid("1e531175-b390-440c-92b6-611d70028b38"), "Giày thể thao" },
                    { new Guid("451387cd-0c22-4bd6-84b5-fdc7468f8525"), "Giày da" },
                    { new Guid("71d134ff-13e3-493c-94d3-652786ce0f7a"), "Dép" },
                    { new Guid("ab54a60d-de70-48d9-882f-4e701ec0c82e"), "Giày boot" },
                    { new Guid("b7eac232-1c5f-4453-93ef-61688d84bfc1"), "Giày cao gót" },
                    { new Guid("e4709a2b-db76-42f7-8514-5f87f25cf6d2"), "Giày lười" },
                    { new Guid("fa90e4ee-d6c9-40b4-8879-fb68a20ee9c5"), "Giày sandal" }
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "ColorCode", "ColorName" },
                values: new object[,]
                {
                    { new Guid("0855abe1-edb1-4516-a9dd-915ce516b0ed"), "#A52A2A", "Nâu" },
                    { new Guid("14f3bb86-6101-4947-953d-582ca0a16754"), "#808080", "Xám" },
                    { new Guid("226f72be-5aca-4765-a980-4327a1c64c84"), "#FFFF00", "Vàng" },
                    { new Guid("40da0a69-2d59-46db-9576-02bdd6b40f88"), "#FFDAB9", "Hồng đào" },
                    { new Guid("6040f820-2b81-4dcf-afa7-34263e5dc25a"), "#FFFFFF", "Trắng" },
                    { new Guid("67ea400c-4b47-4c1e-a94f-dabed54ba065"), "#4B0082", "Chàm" },
                    { new Guid("7350da43-0803-43b0-9b89-3180edb5c52c"), "#008000", "Lục" },
                    { new Guid("8de3048d-d7b9-4fb3-ac4d-555e89d11fb1"), "#E6E6FA", "Tím hoa cà" },
                    { new Guid("95e0c864-0dce-4c76-9a43-3eb7118a7f2d"), "#FF0000", "Đỏ" },
                    { new Guid("a02748aa-763c-418d-9421-5d72fb59b9ba"), "#0000FF", "Lam" },
                    { new Guid("a254ad77-02ad-4dd3-a70c-3375673dd3d7"), "#40E0D0", "Ngọc lam" },
                    { new Guid("c01c054d-117c-4947-b327-fbb148a6b965"), "#800080", "Tím" },
                    { new Guid("df3c5739-7497-4388-b4bc-2e0ee8fdfc73"), "#FFC0CB", "Hồng" },
                    { new Guid("f70d15f6-2db5-48e3-856f-2851bc783484"), "#000000", "Đen" },
                    { new Guid("f8aa9493-dd4a-44a0-8b30-9c275942a0d5"), "#FFA500", "Cam" }
                });

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "Id", "Name", "Status" },
                values: new object[,]
                {
                    { new Guid("15540aa1-9865-4186-9436-e43c8b007a98"), "vnpay", true },
                    { new Guid("8d6648af-e40d-417e-91e1-be5472d88ea3"), "cod", true },
                    { new Guid("c0a20e21-3240-4b29-bf75-c99ecfb726c6"), "momo", true }
                });

            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "Id", "Description", "Name", "PoinsMax", "PointsMin" },
                values: new object[,]
                {
                    { new Guid("873f822d-d1e1-49ad-bc5a-42904cf66d14"), null, "Vàng", 3000000, 1000001 },
                    { new Guid("b9747598-ed96-48d3-87a3-eef9ea73159d"), null, "Kim Cương", 10000000, 3000001 }
                });

            migrationBuilder.UpdateData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2FA6148D-B530-421F-878E-CE1D54BFC6AB",
                column: "ConcurrencyStamp",
                value: "26e8ee8f-1033-4457-9541-128afbbc38f7");

            migrationBuilder.UpdateData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2FA6148D-B530-421F-878E-CE2D54BFC6AB",
                column: "ConcurrencyStamp",
                value: "8985d96c-912f-49af-81aa-42ee337f9766");

            migrationBuilder.UpdateData(
                schema: "security",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2FA6148D-B530-421F-878E-CE4D54BFC6AB",
                column: "ConcurrencyStamp",
                value: "e4f50e15-8942-4c1a-ba1e-a9f89cfd29cc");

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("4276f0ea-bac1-4047-8d0e-1323d92a874e"), "44" });

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("51252a21-8932-4555-bf7f-ffcec328c3a7"), "38" },
                    { new Guid("58b51d20-7d28-451e-9645-615e04278c95"), "41" },
                    { new Guid("753c9050-7664-4b9f-8701-1ec35d4d6262"), "39" },
                    { new Guid("aab408c2-d60f-4bf6-9648-e1758905a642"), "42" },
                    { new Guid("bca09b33-fee6-4859-b950-b859f2294f12"), "40" },
                    { new Guid("e7f96604-8728-45e1-b166-acd2d517d90a"), "43" }
                });

            migrationBuilder.UpdateData(
                schema: "security",
                table: "Users",
                keyColumn: "Id",
                keyValue: "1FA6148D-B530-421F-878E-CE4D54BFC6AB",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "546402e8-092b-4af0-a219-0ae8c92aa283", "a169a150-ab9e-4689-a43b-cf7575fa03ab" });

            migrationBuilder.UpdateData(
                schema: "security",
                table: "Users",
                keyColumn: "Id",
                keyValue: "2FA6148D-B530-421F-878E-CE4D54BFC6AB",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "92dc8c77-caec-4972-9000-3378531af286", "b48bfd43-50c3-4a4e-a764-c94fe0bc1c94" });
        }
    }
}
