using System;
using GameAPIServer.DBModel;

namespace GameAPIServer.Services;

public interface IMasterDataDb
{
    public Task LoadMasterDataAsync();
}