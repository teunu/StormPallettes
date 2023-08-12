using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stormworks_Pallettes_Manager
{
    public static class DefaultPallettes
    {
        public static PalletteCategory deprecated
        {
            get
            {
                var value = new PalletteCategory("V Deprecated Blocks");
                value.default_visible = false;
                value.display_color = ColorTranslator.FromHtml("#249ED6");
                value.explicit_sorting = new List<string> {
                    "torque_gearbox.xml",
                    "fluid_filter.xml",
                    "gate_torque_add.xml",
                    "gate_torque_multimeter.xml",
                    "passenger_seat.xml",
                    "radar_dish.xml",
                    "radar_huge.xml",
                    "radar_large.xml",
                    "radar_sonar_small.xml",
                    "radar_sonar.xml",
                    "radar.xml",
                    "rx_huge.xml",
                    "rx_large.xml",
                    "rx_med.xml",
                    "rx_small.xml",
                    "torque_gearbox2.xml",
                    "water_hose.xml",
                    "water_outlet.xml",
                    "winch_a.xml",
                    "winch_electric.xml",
                    "winch_huge_a.xml",
                    "winch_large_a.xml",
                    "gate_train_junction.xml"
                };
                return value;
            }
        }
        public static PalletteCategory weapons_dlc
        {
            get
            {
                var value = new PalletteCategory("V Weapons Dlc");
                value.default_visible = true;
                value.display_color = ColorTranslator.FromHtml("#3085E6");
                value.explicit_sorting = new List<string> {
                    "inventory_equipment_rifle.xml",
                    "inventory_equipment_rifle_ammo.xml",
                    "inventory_equipment_pistol.xml",
                    "inventory_equipment_pistol_ammo.xml",
                    "inventory_equipment_c4.xml",
                    "inventory_equipment_c4_detonator.xml",
                };
                value.prefix_sorting = new List<string> {
                    "gun_",
                    "warhead_"
                };
                return value;
            }
        }

    }
}
