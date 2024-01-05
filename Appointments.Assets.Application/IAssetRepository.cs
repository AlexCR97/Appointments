using Appointments.Assets.Domain;
using Appointments.Common.Application;

namespace Appointments.Assets.Application;

public interface IAssetRepository : IRepository<Asset>
{
    Task<Asset> GetByPathOrDefaultAsync(AssetPath path);
}
