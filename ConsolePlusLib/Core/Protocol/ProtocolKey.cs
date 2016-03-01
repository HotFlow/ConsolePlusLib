using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Core.Protocol
{
    public enum ProtocolConst
    {
        InitBufferSize = 1024 * 4,  
        ReceiveBufferSize = 1024 * 4,
        SocketTimeOutMS = 60 * 1000
    }

    public enum ProtocolFlag
    {
        [Description("None")]
        None,
        [Description("SQL")]
        SQL,
        [Description("Upload")]
        Upload,
        [Description("Download")]
        Download,
        [Description("RemoteStream")]
        RemoteStream
    }

    public enum ProtocolKey
    {
        [Description("Request")]
        Request,
        [Description("Respond")]
        Respond,
    }
}
