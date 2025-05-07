using MediatR;
using WGL.Auth.Application.DTOs.Account;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Queries.SupplierDiversityGetUsers
{
    public class GetUserSupplierDiversityQuery : IRequest<Response<AdditionalSDDTO>>
    {
        public int UserId { get; set; }
    }

    public class GetUserSupplierDiversityQueryHandler : IRequestHandler<GetUserSupplierDiversityQuery, Response<AdditionalSDDTO>>
    {
        private readonly IAccountRepositoryAsync _repository;
        public GetUserSupplierDiversityQueryHandler(IAccountRepositoryAsync repository)
        {
            _repository = repository;
        }
        public async Task<Response<AdditionalSDDTO>> Handle(GetUserSupplierDiversityQuery getUserSupplierDiversity, CancellationToken cancellationToken)
        {
            var result = await _repository.GetSupplierDiversityUserByIdAsync(getUserSupplierDiversity);
            if (result != null)
            {
                // var response = await _repository.UpdateUserDetailsAfterMailAsync(updateUser.UserId);
                return new Response<AdditionalSDDTO>(result, message: "Successfull");
            }
            else
            {
                return new Response<AdditionalSDDTO>(result, message: "Failed");
            }
        }
    }
}
