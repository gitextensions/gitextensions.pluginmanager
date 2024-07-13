using Neptuo;
using PackageManager.Services;

namespace PackageManager
{
    partial class Args : SelfUpdateService.IArgs, ICloneable<SelfUpdateService.IArgs>
    {
        SelfUpdateService.IArgs ICloneable<SelfUpdateService.IArgs>.Clone()
            => Clone();
    }
}
