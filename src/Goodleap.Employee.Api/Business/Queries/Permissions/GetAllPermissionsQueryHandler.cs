using Goodleap.Employee.Core.Models;
using Goodleap.Employee.Core.Units;
using MediatR;

namespace Goodleap.Employee.Api.Business.Queries.Permissions
{
    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, List<Permission>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPermissionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Permission>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.permissionRepository.GetAllPermissionsAsync();
        }
    }
}
