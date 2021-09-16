﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.Uow
{
    [Service(Lifetime = ServiceLifetime.Singleton)]
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        public IUnitOfWork Current => GetCurrentUnitOfWork();

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAmbientUnitOfWork _ambientUnitOfWork;

        public UnitOfWorkManager(
            IAmbientUnitOfWork ambientUnitOfWork,
            IServiceScopeFactory serviceScopeFactory)
        {
            _ambientUnitOfWork = ambientUnitOfWork;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IUnitOfWork Begin(UnitOfWorkOptions options, bool requiresNew = false)
        {
            Check.NotNull(options, nameof(options));

            // 获得当前的工作单元
            var currentUow = Current; 

            // 如果当前工作单元不为空，并且开发人员明确说明不需要构建新的工作单元时，创建内部工作单元
            if (currentUow != null && !requiresNew)
            {
                return new ChildUnitOfWork(currentUow);
            }

            // 创建新的外部工作单元
            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.Initialize(options);

            return unitOfWork;
        }

        public IUnitOfWork Reserve(string reservationName, bool requiresNew = false)
        {
            Check.NotNull(reservationName, nameof(reservationName));

            if (!requiresNew &&
                _ambientUnitOfWork.UnitOfWork != null &&
                _ambientUnitOfWork.UnitOfWork.IsReservedFor(reservationName))
            {
                return new ChildUnitOfWork(_ambientUnitOfWork.UnitOfWork);
            }

            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.Reserve(reservationName);

            return unitOfWork;
        }

        public void BeginReserved(string reservationName, UnitOfWorkOptions options)
        {
            if (!TryBeginReserved(reservationName, options))
            {
                throw new Exception($"Could not find a reserved unit of work with reservation name: {reservationName}");
            }
        }

        public bool TryBeginReserved(string reservationName, UnitOfWorkOptions options)
        {
            Check.NotNull(reservationName, nameof(reservationName));

            var uow = _ambientUnitOfWork.UnitOfWork;

            //Find reserved unit of work starting from current and going to outers
            while (uow != null && !uow.IsReservedFor(reservationName))
            {
                uow = uow.Outer;
            }

            if (uow == null)
            {
                return false;
            }

            uow.Initialize(options);

            return true;
        }

        private IUnitOfWork GetCurrentUnitOfWork()
        {
            var uow = _ambientUnitOfWork.UnitOfWork;

            //Skip reserved unit of work
            while (uow != null && (uow.IsReserved || uow.IsDisposed || uow.IsCompleted))
            {
                uow = uow.Outer;
            }

            return uow;
        }

        private IUnitOfWork CreateNewUnitOfWork()
        {
            var scope = _serviceScopeFactory.CreateScope();
            try
            {
                var outerUow = _ambientUnitOfWork.UnitOfWork;
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                // 设置当前工作单元的外部工作单元
                unitOfWork.SetOuter(outerUow);

                // 设置最外层的工作单元
                _ambientUnitOfWork.SetUnitOfWork(unitOfWork); 

                unitOfWork.Disposed += (sender, args) =>
                {
                    _ambientUnitOfWork.SetUnitOfWork(outerUow);
                    scope.Dispose();
                };

                return unitOfWork;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }
    }
}
