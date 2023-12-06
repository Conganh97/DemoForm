using System.Data;
using Contracts.Interfaces;
using Dapper;
using MediatR;

namespace Application.Features.Customers.Command
{
    public class RegisterCommand : IRequest<Guid>
    {
        public string cus_person { get; set; }
        public string cus_so_dk { get; set; }
        public string cus_country_code { get; set; }
        public string cus_ngay_cap { get; set; }
        public string cus_noi_cap { get; set; }
        public string nguoi_dai_dien { get; set; }
        public string chuc_vu { get; set; }
        public string cus_ten_chu_hang { get; set; }
        public string cus_dia_chi { get; set; }
        public string customer_name { get; set; }
        public string cus_address { get; set; }
        public string cus_tel { get; set; }
        public string cus_email { get; set; }
        public string cus_fax { get; set; }
        public string email_hoa_don { get; set; }
        public string lon_login_name { get; set; }
        public string lon_login_password { get; set; }
        public string thu_tuc { get; set; }
        public string account_1_cua { get; set; }
        public string password_1_cua { get; set; }


        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
        {
            private readonly IApplicationWriteDbConnection _writeDbConnection;

            public RegisterCommandHandler(IApplicationWriteDbConnection writeDbConnection)
            {
                _writeDbConnection = writeDbConnection;
            }

            public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var queryUser =
                    "INSERT INTO [USER] (LON_USER_NAME, LON_LOGIN_PASSWORD,LON_LOGIN_NAME, THUTUC,ACCOUNT1CUA,PASSWORD1CUA) " +
                    "VALUES (@LON_User_Name, @LON_Login_Password,@LON_Login_Name,@Thu_Tuc,@Account_1_Cua,@Password_1_Cua) " +
                    "INSERT INTO [Customer] (CustomerCode,CUS_Person, CUS_SoDK, CUS_CountryCode, CUS_NgayCap, CUS_NoiCap, " +
                    "NguoiDaiDien, ChucVu, CUS_TenChuHang, CUS_DiaChi, CustomerName, CUS_Address,CUS_Tel,CUS_Email,CUS_Fax,EmailHoaDon) " +
                    "VALUES (@CustomerCode,@cus_person, @cus_sodk, @cus_countrycode, @cus_ngaycap, @cus_noicap, @nguoidaidien, @chucvu, " +
                    "@cus_tenchuhang, @cus_diachi, @customername, @cus_address,@cus_tel,@cus_email,@cus_fax,@emailhoadon)";
                
                var parameters = new DynamicParameters();
                parameters.Add("LON_User_Name", request.lon_login_name, DbType.String);
                parameters.Add("LON_Login_Password", request.lon_login_password, DbType.String);
                parameters.Add("LON_Login_Name", request.cus_person, DbType.String);
                parameters.Add("Thu_Tuc", request.thu_tuc, DbType.String);
                parameters.Add("Account_1_Cua", request.account_1_cua, DbType.String);
                parameters.Add("Password_1_cua", request.password_1_cua, DbType.String);
                parameters.Add("CustomerCode",new Random().NextInt64().ToString());
                parameters.Add("cus_person", request.cus_person != null, DbType.Boolean);
                parameters.Add("cus_sodk", request.cus_so_dk, DbType.String);
                parameters.Add("cus_countrycode", request.cus_country_code, DbType.String);
                parameters.Add("cus_ngaycap", request.cus_ngay_cap, DbType.String);
                parameters.Add("cus_noicap", request.cus_noi_cap, DbType.String);
                parameters.Add("nguoidaidien", request.nguoi_dai_dien, DbType.String);
                parameters.Add("chucvu", request.chuc_vu, DbType.String);
                parameters.Add("cus_tenchuhang", request.cus_ten_chu_hang, DbType.String);
                parameters.Add("cus_diachi", request.cus_dia_chi, DbType.String);
                parameters.Add("customername", request.customer_name, DbType.String);
                parameters.Add("cus_address", request.cus_address, DbType.String);
                parameters.Add("cus_tel", request.cus_tel, DbType.String);
                parameters.Add("cus_email", request.cus_email, DbType.String);
                parameters.Add("cus_fax", request.cus_fax, DbType.String);
                parameters.Add("emailhoadon", request.email_hoa_don, DbType.String);
                
                await _writeDbConnection.ExecuteAsync(queryUser, parameters);
                return new Guid();
            }
        }
    }
}