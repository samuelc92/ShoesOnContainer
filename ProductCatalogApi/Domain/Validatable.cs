using Flunt.Notifications;
using Flunt.Validations;

namespace ProductCatalogApi.Domain
{
    
    public abstract class Validatable : Notifiable, IValidatable
    {
        public abstract void Validate();
    }
}