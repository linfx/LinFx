using LinFx.Extensions.Modules;
using LinFx.Extensions.Uow;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.Test.Uow
{
    public class UnitOfWorkTests
    {
		private readonly ServiceProvider _services;

		public UnitOfWorkTests()
		{
			var services = new ServiceCollection();
			services
				.AddLinFx();

			_services = services.BuildServiceProvider();
		}

		[Fact]
		public async Task UnitOfWorkManager_Reservation_Test()
		{
			var _unitOfWorkManager = _services.GetRequiredService<IUnitOfWorkManager>();

			_unitOfWorkManager.Current.ShouldBeNull();

			using (var uow1 = _unitOfWorkManager.Reserve("Reservation1"))
			{
                _unitOfWorkManager.Current.ShouldBeNull();

				using (var uow2 = _unitOfWorkManager.Begin())
				{
                    // 此时 Current 值是 Uow2 的值。
                    _unitOfWorkManager.Current.ShouldNotBeNull();
                    _unitOfWorkManager.Current.Id.ShouldNotBe(uow1.Id);

					await uow2.CompleteAsync();
				}

                // 这个时候，因为 uow1 是保留工作单元，所以不会被获取到，应该为 null。
                _unitOfWorkManager.Current.ShouldBeNull();

				// 调用了该方法，设置 uow1 的 IsReserved 属性为 false。
				_unitOfWorkManager.BeginReserved("Reservation1");

				// 获得到了值，并且诶它的 Id 是 uow1 的值。
				_unitOfWorkManager.Current.ShouldNotBeNull();
				_unitOfWorkManager.Current.Id.ShouldBe(uow1.Id);

				await uow1.CompleteAsync();
			}

			_unitOfWorkManager.Current.ShouldBeNull();
		}
	}
}
