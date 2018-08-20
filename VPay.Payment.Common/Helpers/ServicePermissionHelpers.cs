using System;

namespace VPay.Payment.Common
{
    public static class ServicePermissionHelpers
    {

        public static (string PermissionName, string Action) GetPermissionInfo(this ServicePermission permission)
        {
            switch (permission)
            {
                case ServicePermission.GetPan:
                    return ("GETPAN", "EDIT");
                case ServicePermission.OpenPreAuth:
                    return ("OPENPREAUTH", "READ");
                case ServicePermission.LoadPan:
                    return ("LOADPAN", "EDIT");
                case ServicePermission.BalanceRequest:
                    return ("BALREQUEST", "READ");
                case ServicePermission.Unload:
                    return ("UNLOAD", "DELETE");
                case ServicePermission.StopPay:
                    return ("STOPPAY", "DELETE");
                case ServicePermission.CancelFax:
                    return ("CANCELFAX", "DELETE");
                case ServicePermission.ChangeFaxNumber:
                    return ("CHANGEFAXNUMBER", "DELETE");
                case ServicePermission.HoldFax:
                    return ("HOLDFAX", "DELETE");
                case ServicePermission.ReleaseFax:
                    return ("RELEASEFAX", "DELETE");
                case ServicePermission.ResendFax:
                    return ("RESENDFAX", "DELETE");
                default:
                    throw new ArgumentOutOfRangeException(nameof(permission), permission, null);
            }
        }

    }
}
