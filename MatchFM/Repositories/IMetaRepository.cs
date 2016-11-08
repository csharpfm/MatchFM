using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchFM.Models;

namespace MatchFM.Repositories
{
    interface IMetaRepository
    {
        Meta FetchById(int id);
        Meta FetchByName(string name);
        Meta FetchByMbId(string mbid);
        bool ExistsByName(string name);
        bool ExistsByMbId(string mbid);
        Meta FetchOrCreateByMbId(Meta meta);
    }
}
