using HttpForwarder.Abstract.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpForwarder.Abstract
{
    public interface IMenuBuilder
    {
        MainMenu GetMenu();
    }
}
