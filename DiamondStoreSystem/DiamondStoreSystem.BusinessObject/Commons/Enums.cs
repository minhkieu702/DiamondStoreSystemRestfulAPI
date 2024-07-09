using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.Commons
{
    public enum ClarityGrade
    {
        I3, I2, I1, SI2, SI1, VS2, VS1, VVS2, VVS1, IF, FL
    }

    public enum ColorGrade
    {
        Z, Y, X, W, V, U, T, S, R, Q, P, O, N, M, L, K, J, I, H, G, F, E, D
    }

    public enum Grade
    {
        Poor, Fair, Good, VeryGood, Excellent
    }

    public enum LabCreated
    {
        Artificial,
        Natural
    }

    public enum Shape
    {
        Round,
        Princess,
        Emerald,
        Marquise,
        Heart,
        Pear,
        Radiant,
        Oval,
        Asscher,
        Cushion
    }

    public enum Material
    {
        PreciousMetals,
        SterlingSilver,
        HypoallergenicMetals,
        Leather,
        Fabric,
        Silicone
    }

    public enum Style
    {
        Ring, Necklace, Pendant, Earring, Shake, Charm, Neck, Straps, Stirrups, GoldenFortune
    }

    public enum OrderStatus
    {
        Cancelled,
        Pending,
        Paid
    }

    public enum PayMethod
    {
        Cash, Online
    }

    public enum Role
    {
        CUSTOMER,
        STAFF,
        MANAGER,
        ADMIN
    }

    public enum WorkingSchedule
    {
        AllDay,
        Morning,
        Afternoon,
        Evenings
    }

    public enum Gender
    {
        Female,
        Male
    }
}
