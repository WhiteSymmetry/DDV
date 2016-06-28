from datetime import datetime

from LargeImages import DDVTileLayout, LayoutLevel


class ParallelLayout(DDVTileLayout):
    def __init__(self):
        super(ParallelLayout, self).__init__()
        self.genome_processed = 0
        # modify layout with an additional bundled column layer
        self.levels = self.levels[:2]  # trim off the others
        self.levels.append(LayoutLevel("ColumnInRow", 32, levels=self.levels))  # [2]
        self.levels[2].thickness = 330  # 100+6+100+6+100+18 = 330  #total row width of 10,560 vs original 10,600
        self.levels[2].padding = 330 - 100
        self.column_offset = 106  # steps inside a column bundle, not exactly the same as bundles steps because of ...
        # ... inter bundle padding of 18 pixels
        self.levels.append(LayoutLevel("RowInTile", 10, levels=self.levels))  # [3]
        self.levels.append(LayoutLevel("XInTile", 3, levels=self.levels))  # [4]
        self.levels.append(LayoutLevel("YInTile", 99, levels=self.levels))  # [5]


    def process_file(self, file1, output_folder, output_file_name, file2=None, file3=None):
        file2 = file1 if file2 is None else file2
        file3 = file1 if file3 is None else file3
        start_time = datetime.now()
        self.image_length = self.read_contigs(file1)
        print("Read first sequence :", datetime.now() - start_time)
        self.prepare_image(self.image_length)
        print("Initialized Image:", datetime.now() - start_time)
        self.draw_nucleotides()
        print("Drew First File:", file1, datetime.now() - start_time)

        # Do inner work for two other files
        self.genome_processed = 1
        self.read_contigs(file2)
        self.draw_nucleotides()
        print("Drew Second File:", file2, datetime.now() - start_time)
        self.genome_processed = 2
        self.read_contigs(file3)
        self.draw_nucleotides()
        print("Drew Third File:", file3, datetime.now() - start_time)

        self.generate_html(file1, output_folder, output_file_name)
        self.output_image(output_folder, output_file_name)
        print("Output Image in:", datetime.now() - start_time)


    def position_on_screen(self, index):
        """ In ParallelLayout, each genome is given a constant x offset in order to interleave the results of each
        genome as it is processed separately.
        """
        x, y = super(ParallelLayout, self).position_on_screen(index)
        return [x + self.column_offset * self.genome_processed, y]

    def draw_titles(self):
        return  # Titles should not be draw in 3 column layout

    def calc_padding(self, total_progress, next_segment_length, multipart_file):
        """Shallow copy of super().calc_padding where space is not allocated for titles"""
        return 0, 0, 0
        # min_gap = (20 + 6) * 100  # 20px font height, + 6px vertical padding  * 100 nt per line
        # if not multipart_file:
        #     return 0, 0, 0
        #
        # for i, current_level in enumerate(self.levels):
        #     if next_segment_length < current_level.chunk_size:
        #         # give a full level of blank space just in case the previous
        #         title_padding = 0 if self.levels[i - 1].chunk_size > min_gap else min_gap
        #         space_remaining = current_level.chunk_size - total_progress % current_level.chunk_size
        #         # sequence comes right up to the edge.  There should always be >= 1 full gap
        #         reset_level = current_level  # bigger reset when close to filling chunk_size
        #         if next_segment_length + title_padding < space_remaining:
        #             reset_level = self.levels[i - 1]
        #             # reset_level = self.levels[i - 1]
        #         reset_padding = 0
        #         if total_progress != 0:  # fill out the remainder so we can start at the beginning
        #             reset_padding = reset_level.chunk_size - total_progress % reset_level.chunk_size
        #         total_padding = total_progress + title_padding + reset_padding + next_segment_length
        #         tail = self.levels[i - 1].chunk_size - total_padding % self.levels[i - 1].chunk_size - 1
        #
        #         return reset_padding, title_padding, tail

        return 0, 0, 0