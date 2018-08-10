# LocalBinaryPattern
## About
This project contains a C# implementation of local binary patterns (LBP) and  median robust extended local binary patterns (MRELBP). We currently support only Windows.

The repository has 3 projects: A demo software (LBP), unit test project (LBP.UnitTests) and class library to make LBP calculations (LBPLibrary).

Current build status:  

[![Build status](https://ci.appveyor.com/api/projects/status/d0c6874wheduojbe?svg=true)](https://ci.appveyor.com/project/sarytky/localbinarypattern-g7kbi)
[![codecov](https://codecov.io/gh/sarytky/LocalBinaryPattern/branch/master/graph/badge.svg)](https://codecov.io/gh/sarytky/LocalBinaryPattern)

## Prerequisites
Demo software can be run using windows without additional installations.
Unit tests and class library are recommended to use through MS Visual Studio.

## Installation
1. Download the repository to your computer.
2. Extract folder.
3. Navigate to LocalBinaryPattern/LBP/LBP/bin/debug and run LBP.exe

## Application usage
Application can be used to calculate LBP images or MRELBP images either for stack of images or single images. Parameters can be defined in the main menu, or calculation can be started instantly using default parameters. Before calculation, image, or path to image stack and path for saving results must be specified.

## Outputs: 
LBP calculation: LBP image and histogram of mapped LBP features

MRELBP: 3 MRELBP images (small, radial, large) and histogram including center, large, small and radial features.

Histograms are outputted as .csv file and binary .dat file

## Examples
Original image and LBP image (8 neighbours, radius 2):

![Original image](https://github.com/MIPT-Oulu/LocalBinaryPattern/blob/master/pictures/MRI_original.png) ![LBP image](https://github.com/MIPT-Oulu/LocalBinaryPattern/blob/master/pictures/MRI_LBP.png)

MRELBP images for small radius, radial image and large radius: (8 neighbours, small radius 2, large radius 4, filter sizes 3)

![Small radius](https://github.com/MIPT-Oulu/LocalBinaryPattern/blob/master/pictures/MRI_small.png) ![Radial image](https://github.com/MIPT-Oulu/LocalBinaryPattern/blob/master/pictures/MRI_radial.png) ![Large radius](https://github.com/MIPT-Oulu/LocalBinaryPattern/blob/master/pictures/MRI_large.png)

## License

This software is distributed under the MIT License.

## Citation
```
@misc{Rytky2018,
  author = {Rytky, Santeri and Tiulpin, Aleksei and Frondelius, Tuomas and Saarakkala Simo},
  title = {Local Binary Pattern},
  year = {2018},
  publisher = {GitHub},
  journal = {GitHub repository},
  howpublished = {\url{https://github.com/MIPT-Oulu/LocalBinaryPattern}},
}
```
