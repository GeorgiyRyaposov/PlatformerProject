using Common.Data;

namespace Common.Views
{
    public interface IGameObjectView
    {
        Id TemplateId { get; set; }
        Id InstanceId { get; set; }
    }
}