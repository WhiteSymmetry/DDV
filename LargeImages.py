"""
This is an optional addon to DDV written in Python that allows you to generate a single image
for an entire genome.  It was necessary to switch platforms and languages because of intrinsic
limitations in the size of image that could be handled by: C#, DirectX, Win2D, GDI+, WIC, SharpDX,
or Direct2D. We tried a lot of options.

This python file contains basic image handling methods.  It also contains a re-implementation of
Josiah's "Tiled Layout" algorithm which is also in DDVLayoutManager.cs.
"""

from PIL import Image, ImageDraw, ImageFont
from datetime import datetime


def hello_world():
    starttime = datetime.now()

    img = Image.new('RGB', (10000, 10000), "white")
    draw = ImageDraw.Draw(img)
    pixels = img.load()
    font = ImageFont.truetype("calibri.ttf", 16)

    for i in range(img.size[0]):
        j = 0
        while j < img.size[1]:
            pixels[i, j] = (255, 0, 0)
            j += 1
            pixels[i, j] = (0, 0, 255)
            j += 1
    draw.text((1000, 1000), "Hello Beginning!", (255, 255, 255), font=font)
    draw.text((9000, 9000), "Goodbye Ending!", (255, 255, 255), font=font)

    del pixels
    del draw

    img.save('output.png', 'PNG')

    del img

    print("Finished Array in:", datetime.now() - starttime)

if __name__ == '__main__':
    hello_world()
