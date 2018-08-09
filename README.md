# LocalBinaryPattern
Calculates Median robust extended local binary pattern (MRELBP) or local binary pattern (LBP) from images.
Repository contains 3 projects: A demo software (LBP), unit test project (LBP.UnitTests) and class library to make LBP calculations (LBPLibrary).

## Prerequisites
Demo software can be run using windows without additional installations.
Unit tests and class library are recommended to use through MS Visual Studio.

## Installation
1. Download the repository to your computer.
2. Extract folder.
3. Navigate to LocalBinaryPattern/LBP/LBP/publish and run LBP.application

## Application usage
Application can be used to calculate LBP images or MRELBP images either for stack of images or single images. Parameters can be defined in the main menu, or calculation can be started instantly using default parameters. Before calculation, image, or path to image stack and path for saving results must be specified.

## Examples
Original image and LBP image (8 neighbours, radius 2):

![Original image](https://github.com/MIPT-Oulu/LocalBinaryPattern/blob/master/pictures/MRI_original.png) ![LBP image](https://github.com/MIPT-Oulu/LocalBinaryPattern/blob/master/pictures/MRI_LBP.png)

MRELBP images for small radius, radial image and large radius: (8 neighbours, small radius 2, large radius 4, filter sizes 3)

![Small radius](https://github.com/MIPT-Oulu/LocalBinaryPattern/blob/master/pictures/MRI_small.png) ![Radial image](https://github.com/MIPT-Oulu/LocalBinaryPattern/blob/master/pictures/MRI_radial.png) ![Large radius](https://github.com/MIPT-Oulu/LocalBinaryPattern/blob/master/pictures/MRI_large.png)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments

MRELBP is based on method published in: *Liu et. al. Median Robust Extended Local Binary Pattern for Texture Classification (2016)*
