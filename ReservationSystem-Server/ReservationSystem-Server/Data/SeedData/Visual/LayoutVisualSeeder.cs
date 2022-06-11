using ReservationSystem_Server.Data.Visual.Layout;
using ReservationSystem_Server.Helper.DataSeed;

namespace ReservationSystem_Server.Data.SeedData.Visual;

public static class LayoutVisualSeeder
{
    [DataSeeder]
    private static void SeedRectangles(List<RectangleVisual> rectangles)
    {
        RectangleVisual Create(int id, float x, float y, float width, float height, byte r, byte g, byte b, byte a, int? tableTypeId = null)
        {
            return new RectangleVisual
            {
                Id = id,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                R = r,
                G = g,
                B = b,
                A = a,
                TableTypeVisualId = tableTypeId
            };
        }

        rectangles.AddRange(new[]
        {
            Create(1, 48.241886F, 34.69217F, 25, 25, 144, 19, 254, 255), // for main area
            Create(2, 48.138847F, 9.550489F, 25, 25, 126, 211, 33, 255), // for outside area
            Create(3, 4.620685F, 9.3089905F, 43.75644F, 37.986217F, 80, 227, 194, 255), // for balcony area
            Create(4, 0.051519834F, 5.225631F, 99.94848F, 89.64534F, 139, 87, 42, 255, 1), // for 2-seater main part
            Create(5, 6.7168984F, 0.012075072F, 86.17336F, 9.997585F, 208, 2, 27, 255, 1), // for 2-seater left part
            Create(6, 7.9694743F, 89.76178F, 86.37944F, 10.218387F, 208, 2, 27, 255, 1), // for 2-seater right part
        });
    }

    [DataSeeder]
    private static void SeedTableTypes(List<TableTypeVisual> tableTypes)
    {
        tableTypes.Add(new TableTypeVisual
        {
            Id = 1, // 5
            Name = "2-seater",
            Seats = 2,
            Width = 5,
            Height = 7
        });
    }

    [DataSeeder]
    private static void SeedAreaVisuals(List<RestaurantAreaVisual> visuals)
    {
        visuals.AddRange(new[]
        {
            new RestaurantAreaVisual
            {
                AreaId = 1,
                RectId = 1
            },
            new RestaurantAreaVisual
            {
                AreaId = 2,
                RectId = 2
            },
            new RestaurantAreaVisual
            {
                AreaId = 3,
                RectId = 3
            }
        });
    }

    /*DATA
     TableId,TableTypeId,X,Y,Rotation
     2,5,47.552807,39.464195,0 --M1
    3,5,73.20969,69.44874,0 -- M2
    4,5,39.92787,70.47913,0 -- M3
    5,5,3.5548687,70.47913,0 -- M4
    6,5,71.04585,38.12468,0 -- M5
    7,5,26.120556,39.567234,0 -- M6
    8,5,2.833591,38.22772,0 -- M7
    9,5,70.32458,1.7516744,0 -- M8
    10,5,38.691395,1.9577538,0 -- M9
    11,5,3.0396702,2.1638331,0 -- M10
    12,5,54.147346,35.548687,0 -- O1
    13,5,31.478619,36.166924,0 -- O2
    14,5,40.75219,71.92169,90 -- O3
    15,5,41.267387,0.61823803,90 -- O4
    16,5,77.43431,35.651726,0 -- O5
    17,5,6.7490983,36.269962,0 -- O6
    18,5,6.7490983,69.963936,135 -- O7
    19,5,73.724884,69.654816,45 -- O8
    20,5,73.724884,2.5759919,135 -- O9
    21,5,4.997424,2.5759919,45 -- O10
    22,5,23.441525,36.438354,90 -- B1
    23,5,77.53735,72.75802,0 -- B2
    24,5,77.022156,36.67574,0 -- B3
    25,5,77.12519,5.459819,0 -- B4
    26,5,43.1221,70.146805,0 -- B5
    27,5,42.916023,35.607513,0 -- B6
    28,5,43.94642,4.86636,0 -- B7
    29,5,4.585265,68.485115,0 -- B8
    30,5,4.379186,34.539288,0 -- B9
    31,5,3.5548687,4.035518,0 -- B10
     */
    [DataSeeder]
    private static void SeedTableVisuals(List<TableVisual> visuals)
    {
        TableVisual Create(int id, int typeId, float x, float y, int rotation)
        {
            return new TableVisual
            {
                TableId = id,
                TableTypeId = typeId,
                X = x,
                Y = y,
                Rotation = rotation
            };
        }

        visuals.AddRange(new[]
        {
            Create(2, 1, 47.552807F, 39.464195F, 0),
            Create(3, 1, 73.20969F, 69.44874F, 0),
            Create(4, 1, 39.92787F, 70.47913F, 0),
            Create(5, 1, 3.5548687F, 70.47913F, 0),
            Create(6, 1, 71.04585F, 38.12468F, 0),
            Create(7, 1, 26.120556F, 39.567234F, 0),
            Create(8, 1, 2.833591F, 38.22772F, 0),
            Create(9, 1, 70.32458F, 1.7516744F, 0),
            Create(10, 1, 38.691395F, 1.9577538F, 0),
            Create(11, 1, 3.0396702F, 2.1638331F, 0),
            Create(12, 1, 54.147346F, 35.548687F, 0),
            Create(13, 1, 31.478619F, 36.166924F, 0),
            Create(14, 1, 40.75219F, 71.92169F, 90),
            Create(15, 1, 41.267387F, 0.61823803F, 90),
            Create(16, 1, 77.43431F, 35.651726F, 0),
            Create(17, 1, 6.7490983F, 36.269962F, 0),
            Create(18, 1, 6.7490983F, 69.963936F, 135),
            Create(19, 1, 73.724884F, 69.654816F, 45),
            Create(20, 1, 73.724884F, 2.5759919F, 135),
            Create(21, 1, 4.997424F, 2.5759919F, 45),
            Create(22, 1, 23.441525F, 36.438354F, 90),
            Create(23, 1, 77.53735F, 72.75802F, 0),
            Create(24, 1, 77.022156F, 36.67574F, 0),
            Create(25, 1, 77.12519F, 5.459819F, 0),
            Create(26, 1, 43.1221F, 70.146805F, 0),
            Create(27, 1, 42.916023F, 35.607513F, 0),
            Create(28, 1, 43.94642F, 4.86636F, 0),
            Create(29, 1, 4.585265F, 68.485115F, 0),
            Create(30, 1, 4.379186F, 34.539288F, 0),
            Create(31, 1, 3.5548687F, 4.035518F, 0)
        });
    }
}