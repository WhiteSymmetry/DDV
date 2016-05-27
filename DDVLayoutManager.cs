﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDV
{
/** Consistent rules for laying out nucleotides in a recursively tiled, left to 
*   right manner using the following numbers:
     
 name       modulo size    padding  thickness (derived)
---------    ----  -----   -------   ----------------
XInColumn    100  1        0           1
LineInColumn 1000 100nt    0           1
ColumnInRow  100  100KB    4           104    100 * 1 + 4
RowInTile    10   10MB     40          1040   1000 * 1 + 40
Tile X       3    300MB    400         10800  (100 * 104) + 400
Tile Y       4    1200MB   1600        12000   (10*1040) + 1600


Example Problem:  Index: 751,270,123
The order in which the levels are calculated is not important.
XInColumn     751,270,123 % 100 = 23
LineInColumn  7512701 % 1000  = 701
ColumnInRow   7512 % 100 = 12
RowInTile     75 % 10 = 5
Tile X        2
Tile Y        0

To go from mouse coordinates to a genome index, you must use the inverse function: index_from_screen()

coordinate_in_chunk(index, size, modulo){
	return (int)(index / size) % modulo
}

position_on_screen(index, padding){
	xy = [0, 0]
	for i, level in enumerate(levels)
		part = i % 2
		xy[part] += level.thickness * coordinate_in_chunk(index, level.count, level.modulo)
}

/ ** The order of level that x and y are decomposed is important so that 
* the padding is subtracted rather than being counted towards the index.
* /
index_from_screen(x, y){
	index_from_yx = 0
	yx_remaining = [y, x]  //order reversed
	for level i, level in enumerate(reverse(levels)):
		part = i % 2
		number_of_full_increments = (int)(yx_remaining[part] / level.thickness)
		index_from_yx += level.modulo * number_of_full_increments // add total nulceotide size for every full increment of this level e.g. Tile Y height
		yx_remaining[part] -= number_of_full_increments * level.thickness  //subtract the credited coordinates to shift to relative coordinates in that level
	return index_from_yx
}
*/


    /** Simple POD object for Levels table **/
    class LayoutLevel
    {
        public int modulo, chunk_size, padding, thickness;
        public LayoutLevel(string name, int modulo, int chunk_size, int padding, int thickness)
        {
            this.modulo = modulo;
            this.chunk_size = chunk_size;
            this.padding = padding;
            this.thickness = thickness;
        }
    }


    class DDVLayoutManager
    {
        List<LayoutLevel> levels;
        public DDVLayoutManager()
        {
            levels = new List<LayoutLevel>();
            /**  name       modulo size    padding  thickness (derived)
                ---------    ----  -----   -------   ----------------
                XInColumn    100  1        0           1
                LineInColumn 1000 100nt    0           1
                ColumnInRow  100  100KB    4           104    100 * 1 + 4
                RowInTile    10   10MB     40          1040   1000 * 1 + 40
                Tile X       3    300MB    400         10800  (100 * 104) + 400
                Tile Y       4    1200MB   1600        12000   (10*1040) + 1600
             */
            levels.Add(new LayoutLevel("XInColumn", 100, 1, 0, 1));
            levels.Add(new LayoutLevel("LineInColumn", 1000, 100, 0, 1));
            levels.Add(new LayoutLevel("ColumnInRow", 100, 100000, 4, 104));
            levels.Add(new LayoutLevel("RowInTile", 10, 10000000, 40, 1040));
            levels.Add(new LayoutLevel("Tile X", 3, 300000000, 400, 10800 ));
            levels.Add(new LayoutLevel("Tile Y", 4, 1200000000, 1600, 12000));
        }

        public int[] position_on_screen(int index)
        {
	        int[] xy = new int[]{0, 0};
	        for (int i = 0; i < this.levels.Count; ++i)
            {
                LayoutLevel level = this.levels[i];
		        int part = i % 2;
                int coordinate_in_chunk = (int)(index / level.chunk_size) % level.modulo;
		        xy[part] += level.thickness * coordinate_in_chunk;
            }
            return xy;
        }

        public int index_from_screen(int x, int y)
        {
	        int index_from_xy = 0;
	        int[] xy_remaining = {x, y}; 
	        for (int i = this.levels.Count-1; i >= 0; --i) //reverse
            {
                LayoutLevel level = this.levels[i];
		        int part = i % 2;
		        int number_of_full_increments = (int)(xy_remaining[part] / level.thickness);
                index_from_xy += level.chunk_size * number_of_full_increments; // add total nulceotide size for every full increment of this level e.g. Tile Y height
		        xy_remaining[part] -= number_of_full_increments * level.thickness;  //subtract the credited coordinates to shift to relative coordinates in that level
            }
            return index_from_xy;
        }

        /** Similar to position_on_screen(index) but it instead returns the largest x and y values that the layout will need from
         * any index in between 0 and last_index.
         */ 
        public int[] max_dimensions(int last_index)
        {
            int[] xy = new int[] { 0, 0 };
            for (int i = 0; i < this.levels.Count; ++i)
            {
                LayoutLevel level = this.levels[i];
                int part = i % 2;
                int coordinate_in_chunk = Math.Min((int)(Math.Ceiling( (double)last_index / level.chunk_size)), level.modulo);  //how many of these will you need up to a full modulo worth
                if (coordinate_in_chunk > 1) { 
                    xy[part] = Math.Max(xy[part], level.thickness * coordinate_in_chunk); // not cumulative, just take the max size for either x or y
                }
            }
            return xy;
        }

    }


}