"""
self.is an optional addon to DDV written in Python that allows you to generate a single image
for an entire genome.  It was necessary to switch platforms and languages because of intrinsic
limitations in the size of image that could be handled by: C#, DirectX, Win2D, GDI+, WIC, SharpDX,
or Direct2D. We tried a lot of options.

self.python file contains basic image handling methods.  It also contains a re-implementation of
Josiah's "Tiled Layout" algorithm which is also in DDVLayoutManager.cs.
"""
import math
from PIL import Image, ImageDraw, ImageFont
from datetime import datetime
from collections import defaultdict

# Original DDV Colors
palette = defaultdict(lambda: (0, 0, 0))
palette['A'] = (255, 0, 0)
palette['a'] = (255, 0, 0)  # A
palette['G'] = (0, 255, 0)
palette['g'] = (0, 255, 0)  # G
palette['T'] = (250, 240, 114)
palette['t'] = (250, 240, 114)  # T
palette['C'] = (0, 0, 255)
palette['c'] = (0, 0, 255)  # C
palette['N'] = (30, 30, 30)
palette['n'] = (30, 30, 30)  # N


def hello_world():
    starttime = datetime.now()

    image = Image.new('RGB', (10000, 10000), "white")
    draw = ImageDraw.Draw(image)
    pixels = image.load()
    font = ImageFont.truetype("calibri.ttf", 16)

    for i in range(image.size[0]):
        j = 0
        while j < image.size[1]:
            pixels[i, j] = (255, 0, 0)
            j += 1
            pixels[i, j] = (0, 0, 255)
            j += 1
    draw.text((1000, 1000), "Hello Beginning!", (255, 255, 255), font=font)
    draw.text((9000, 9000), "Goodbye Ending!", (255, 255, 255), font=font)

    del pixels
    del draw
    image.save('output.png', 'PNG')
    del image

    print("Finished Array in:", datetime.now() - starttime)



class LayoutLevel:
    def __init__(self, name, modulo, chunk_size=None, padding=0, thickness=1, levels=None):
        self.modulo = modulo
        if chunk_size is not None:
            self.chunk_size = chunk_size
            self.padding = padding
            self.thickness = thickness
        else:
            child = levels[-1]
            self.chunk_size = child.modulo * child.chunk_size
            self.padding = 6 * int(3 ** (len(levels) - 2))  # third level (count=2) should be 6, then 18
            last_parallel = levels[-2]
            self.thickness = last_parallel.modulo * last_parallel.thickness + self.padding


class Contig:
    def __init__(self, name, seq, reset_padding, title_padding, tail_padding):
        self.name = name
        self.seq = seq
        self.reset_padding = reset_padding
        self.title_padding = title_padding
        self.tail_padding = tail_padding


class DDVTileLayout:
    def __init__(self):
        self.image = None
        self.draw = None
        self.pixels = None
        self.contigs = []
        # noinspection PyListCreation
        self.levels = [
            LayoutLevel("XInColumn", 100, 1),
            LayoutLevel("LineInColumn", 1000, 100)
        ]
        self.levels.append(LayoutLevel("ColumnInRow", 100, levels=self.levels))
        self.levels.append(LayoutLevel("RowInTile", 10, levels=self.levels))
        self.levels.append(LayoutLevel("XInTile", 3, levels=self.levels))
        self.levels.append(LayoutLevel("YInTile", 4, levels=self.levels))
        self.levels.append(LayoutLevel("TileColumn", 9, levels=self.levels))
        self.levels.append(LayoutLevel("TileRow", 999, levels=self.levels))

    def process_file(self, input_file_name, output_file_name):
        start_time = datetime.now()
        image_length = self.read_contigs(input_file_name)
        print("Read contigs :", datetime.now() - start_time)
        self.prepare_image(image_length)
        print("Initialized Image:", datetime.now() - start_time)
        total_progress = 0

        if image_length > 300000000:
            positioner = self.position_on_screen_big  # big images
        else:
            positioner = self.position_on_screen  # small images

        # Layout contigs one at a time
        for contig in self.contigs:
            total_progress += contig.reset_padding + contig.title_padding
            # worker.ReportProgress((int) (nucleotidesProcessed += contig.len(seq))) # doesn't include padding
            try:
                for c in contig.seq:
                    x, y = positioner(total_progress)
                    total_progress += 1
                    self.draw_pixel(c, x, y)
            except IndexError as err:
                print("x", x, "y", y)
                print(err)
            total_progress += contig.tail_padding  # add trailing white space after the contig sequence body
        print("Drew Nucleotides:", datetime.now() - start_time)

        # if len(self.contigs) > 1:
        #     self.draw_titles()
        self.output_image(output_file_name)
        print("Output Image in:", datetime.now() - start_time)


    def read_contigs(self, input_file_name):
        multipart_file = False
        self.contigs = []
        total_progress = 0
        current_name = ""
        seq_collection = []

        # Pre-read generates an array of contigs with labels and sequences
        with open(input_file_name, 'r') as streamFASTAFile:
            for read in streamFASTAFile.read().splitlines():
                if read == "":
                    continue
                if read[0] == ">":
                    # correct the padding on first entry before doing this one
                    if len(seq_collection) == 1:
                        reset, title, tail = self.calc_padding(0, len(self.contigs[0].seq), True)
                        self.contigs[0].reset_padding, self.contigs[0].title_padding, self.contigs[0].tail_padding = reset, title, tail
                        total_progress = reset + title + tail + len(self.contigs[0].seq)
                        # this statement will always trigger len() > 0 as well to complete the current contig
                    if len(seq_collection) > 0:
                        multipart_file = True
                        sequence = "".join(seq_collection)
                        seq_collection = []  # clear
                        reset, title, tail = self.calc_padding(total_progress, len(sequence), multipart_file)
                        self.contigs.append(Contig(current_name, sequence, reset, title, tail))
                        total_progress += reset + title + tail + len(sequence)

                        # worker.ReportProgress((int)total_progress)
                    current_name = read[1: -1]  # between > and \n
                else:
                    # collects the sequence to be stored in the contig, constant time performance don't concat strings!
                    seq_collection.append(read)

        # add the last contig to the list
        sequence = "".join(seq_collection)
        reset, title, tail = self.calc_padding(total_progress, len(sequence), multipart_file)
        self.contigs.append(Contig(current_name, sequence, reset, title, tail))
        return total_progress + reset + title + tail + len(sequence)


    def prepare_image(self, image_length):
        width, height = self.max_dimensions(image_length)
        self.image = Image.new('RGB', (width, height), "white")
        self.draw = ImageDraw.Draw(self.image)
        self.pixels = self.image.load()


    def calc_padding(self, total_progress, next_segment_length, multipart_file):
        min_gap = (20 + 6) * 100  # 20px font height, + 6px vertical padding  * 100 nt per line
        if not multipart_file:
            return 0, 0, 0
        
        for i, current_level in enumerate(self.levels):
            if next_segment_length + min_gap < current_level.chunk_size:
                # give a full level of blank space just in case the previous
                title_padding = max(min_gap, self.levels[i - 1].chunk_size)
                space_remaining = current_level.chunk_size - total_progress % current_level.chunk_size
                # sequence comes right up to the edge.  There should always be >= 1 full gap
                reset_level = current_level  # bigger reset when close to filling chunk_size
                if next_segment_length + title_padding < space_remaining:
                    reset_level = self.levels[i - 1] 
                reset_padding = 0
                if total_progress != 0:  # fill out the remainder so we can start at the beginning
                    reset_padding = reset_level.chunk_size - total_progress % reset_level.chunk_size
                total_padding = total_progress + title_padding + reset_padding + next_segment_length
                tail = self.levels[i - 1].chunk_size - total_padding % self.levels[i - 1].chunk_size - 1

                return reset_padding, title_padding, tail

        return 0, 0, 0


    def position_on_screen(self, index):
        """ Readable unoptimized version:
        xy = [0, 0]
        for i, level in enumerate(self.levels):
            if index < level.chunk_size:
                return xy
            part = i % 2
            coordinate_in_chunk = int(index / level.chunk_size) % level.modulo
            xy[part] += level.thickness * coordinate_in_chunk
        """
        # Less readable
        # x = self.levels[0].thickness * (int(index / self.levels[0].chunk_size) % self.levels[0].modulo)
        # x+= self.levels[2].thickness * (int(index / self.levels[2].chunk_size) % self.levels[2].modulo)
        # y = self.levels[1].thickness * (int(index / self.levels[1].chunk_size) % self.levels[1].modulo)
        # y+= self.levels[3].thickness * (int(index / self.levels[3].chunk_size) % self.levels[3].modulo)

        x = index % 100 + 106 * ((index // 100000) % 100) + 10654 * (index // 100000000)  # % 3)
        y = (index // 100) % 1000 + 1018 * ((index // 10000000) % 10)  # + 10342 * ((index // 300000000) % 4)
        return x, y

    def position_on_screen_big(self, index):
        # 10654 * 3 + 486 padding = 32448
        x = index % 100 + 106 * ((index // 100000) % 100) + 10654 * ((index // 100000000) % 3) + \
            32448 * (index // 1200000000)  # % 9 #this will continue tile columns indefinitely (8 needed 4 human genome)
        y = (index // 100) % 1000 + 1018 * ((index // 10000000) % 10) + 10342 * ((index // 300000000) % 4)
        # + 42826 * (index // 10800000000)  # 10342 * 4 + 1458 padding = 42826
        return x, y

    def draw_pixel(self, character, x, y):
        self.pixels[x, y] = palette[character]


    def draw_titles(self):
        total_progress = 0
        for contig in self.contigs:
            total_progress += contig.reset_padding  # is to move the cursor to the right line for a large title
            self.draw_title(total_progress, contig)
            total_progress += contig.title_padding + len(contig.seq) + contig.tail_padding

    def draw_title(self, seqAndPadding, contig):
        font = ImageFont.truetype("calibri.ttf", 16)
        # TODO: from C sharp
        self.draw.text((1000, 1000), "Hello Beginning!", (255, 255, 255), font=font)
        self.draw.text((9000, 9000), "Goodbye Ending!", (255, 255, 255), font=font)


    def output_image(self, output_file_name):
        del self.pixels
        del self.draw
        self.image.save(output_file_name, 'PNG')
        del self.image


    def max_dimensions(self, image_length):
        """ Uses Tile Layout to find the largest chunk size in each dimension (XY) that the
        image_length will reach
        :param image_length: includes sequence length and padding from self.read_contigs()
        :return: width and height needed
        """
        width_height = [0, 0]
        for i, level in enumerate(self.levels):
            part = i % 2
            # how many of these will you need up to a full modulo worth
            coordinate_in_chunk = min(int(math.ceil(image_length / float(level.chunk_size))), level.modulo)
            if coordinate_in_chunk > 1:
                # not cumulative, just take the max size for either x or y
                width_height[part] = max(width_height[part], level.thickness * coordinate_in_chunk)
        return width_height


if __name__ == '__main__':
    # input_file_name, output_file_name = 'sequence.fa', 'output.png'

    layout = DDVTileLayout()
    # layout.process_file('Animalia_Mammalia_Homo_Sapiens_GRCH38_chr20.fa', 'ch20-2.png')
    # layout.process_file('Animalia_Mammalia_Homo_Sapiens_GRCH38_chr1.fa', 'chr1 Human.png')
    layout.process_file('Human selenoproteins.fa', 'selenoproteins.png')

