using Domain.Entities;
using Shared.Contracts;

namespace Domain.Abstractions.Interfaces.Repositories
{
    public interface IHourRepository
    {
        Task<Hour> Create(HourRequest hour); //TODO:  Інтерфейси повинні бути в папці Abstracts або Interfaces, а не в папці Repositories. Це дозволить розділити абстракції від реалізацій.
        Task<int> Delete(int id, DateTime date);
        Task<int> Update(int id, float hours, DateTime date);
    }
}