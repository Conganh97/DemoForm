using System.Data;
using Contracts.Interfaces;
using Dapper;
using MediatR;

namespace Application.Features.Customers.Query
{
    public class LoginCommand : IRequest<LoginCommand.UserDetails>
    {
        public string UserName { get; set; }
        public string Password { get; set; }


        public class LoginCommandHandler : IRequestHandler<LoginCommand, UserDetails>
        {
            private readonly IApplicationReadDbConnection _readDbConnection;

            public LoginCommandHandler(IApplicationReadDbConnection readDbConnection)
            {
                _readDbConnection = readDbConnection;
            }

            public async Task<UserDetails> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var query =
                    " SELECT * FROM [USER] WHERE LON_User_Name = @LON_User_Name AND LON_Login_Password = @LON_Login_Password";

                var parameters = new DynamicParameters();
                parameters.Add("LON_User_Name", request.UserName, DbType.String);
                parameters.Add("LON_Login_Password", request.Password, DbType.String);
                var user = await _readDbConnection.QueryFirstOrDefaultAsync<UserDetails>(query, parameters, cancellationToken: cancellationToken);
                return user;
            }
        }

        public class UserDetails
        {
            public string LON_User_Name { get; set; }
            public string LON_Login_name { get; set; }
            public string LON_Mail { get; set; }
            public string LON_Address { get; set; }
            public string LON_Cellphone { get; set; }
        }
    }
}