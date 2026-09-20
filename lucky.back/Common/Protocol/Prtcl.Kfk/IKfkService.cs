using Lucky.BaseModel.Model;

namespace Prtcl.Kfk
{
    public interface IKfkService
    {
        void Init();

        Task PublishAsync(KfkMsgModel msg);
    }
}
