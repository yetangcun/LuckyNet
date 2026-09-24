using Lucky.BaseModel.Model;

namespace Prtcl.Kfk
{
    public interface IKfkService
    {
        void Init();

        Task PublishAsync(KfkMsgModel msg);

        Task ConsumerAsync(IEnumerable<string> tpcs, IMqConsumerHdl hdl, CancellationToken cancelToken = default);
    }
}
