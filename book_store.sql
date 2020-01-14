-- phpMyAdmin SQL Dump
-- version 4.9.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 14, 2020 at 12:28 AM
-- Server version: 10.4.8-MariaDB
-- PHP Version: 7.3.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `book_store`
--

-- --------------------------------------------------------

--
-- Table structure for table `author`
--

CREATE TABLE `author` (
  `id` int(11) NOT NULL,
  `name` varchar(255) COLLATE utf8_hungarian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `author`
--

INSERT INTO `author` (`id`, `name`) VALUES
(1, 'Test_Author'),
(2, 'wdf'),
(3, 'fsdfsd'),
(4, 'dsfs'),
(5, 'dsfgsg'),
(6, 'hfdh'),
(7, 'sdfgdf'),
(8, 'dfgd'),
(9, 'lkdélkf'),
(10, 'dsaf'),
(11, 'kldfmél'),
(12, 'asdas'),
(13, 'dsafsdf'),
(14, 'sdasd'),
(15, 'édfmgélm');

-- --------------------------------------------------------

--
-- Table structure for table `books`
--

CREATE TABLE `books` (
  `id` int(11) NOT NULL,
  `title` varchar(255) COLLATE utf8_hungarian_ci NOT NULL,
  `author` int(11) NOT NULL,
  `publisher` int(11) NOT NULL,
  `ISBN` varchar(13) COLLATE utf8_hungarian_ci NOT NULL,
  `genre` int(11) NOT NULL,
  `description` text COLLATE utf8_hungarian_ci NOT NULL,
  `publish_year` year(4) NOT NULL,
  `price` int(11) NOT NULL,
  `on_storage` int(11) NOT NULL DEFAULT 0,
  `del` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `books`
--

INSERT INTO `books` (`id`, `title`, `author`, `publisher`, `ISBN`, `genre`, `description`, `publish_year`, `price`, `on_storage`, `del`) VALUES
(1, 'Test_Book', 1, 1, '9789634194583', 1, 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer lectus tellus, egestas ut sagittis porttitor, faucibus eget neque. Etiam iaculis neque dui, vitae venenatis nisl sagittis vel. Suspendisse consectetur mollis augue et sollicitudin.\r\n\r\nUt quis velit sit amet metus porta elementum pellentesque quis metus. Cras iaculis nisi nec risus tristique, sit amet auctor ligula lobortis. Donec a enim et mauris tincidunt mattis. Nunc placerat fermentum convallis. Etiam ut mollis augue. Nullam elementum urna in lobortis scelerisque. Morbi non mattis felis. Suspendisse porta sapien at purus viverra imperdiet. Etiam malesuada purus eu mollis ultrices. Vivamus accumsan eros eu interdum eleifend.', 2018, 3400, 2, 0),
(2, 'Test_Book2', 1, 1, '9789634294583', 1, 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer lectus tellus, egestas ut sagittis porttitor, faucibus eget neque. Etiam iaculis neque dui, vitae venenatis nisl sagittis vel. Suspendisse consectetur mollis augue et sollicitudin.', 2019, 3300, 2, 1);

-- --------------------------------------------------------

--
-- Table structure for table `genre`
--

CREATE TABLE `genre` (
  `id` int(11) NOT NULL,
  `name` varchar(255) COLLATE utf8_hungarian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `genre`
--

INSERT INTO `genre` (`id`, `name`) VALUES
(1, 'Test_Genre'),
(2, 'sdfs'),
(3, 'sdgsd'),
(4, 'sdfsd'),
(5, 'fdh'),
(6, 'gdfg'),
(7, 'dfgdfg'),
(8, 'édlfgél'),
(9, 'asdf'),
(10, 'dfé,lmgém'),
(11, 'ad'),
(12, 'sdfsf'),
(13, 'asdasd'),
(14, 'ldf,géld');

-- --------------------------------------------------------

--
-- Table structure for table `publisher`
--

CREATE TABLE `publisher` (
  `id` int(11) NOT NULL,
  `name` varchar(255) COLLATE utf8_hungarian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `publisher`
--

INSERT INTO `publisher` (`id`, `name`) VALUES
(1, 'Test_Publisher'),
(2, 'dfsfs'),
(3, 'sdfsg'),
(4, 'sdfs'),
(5, 'sdfgsd'),
(6, 'fgdh'),
(7, 'dfdfgd'),
(8, 'dfgdg'),
(9, 'él,géldf,g'),
(10, 'kndénfg'),
(11, 'asda'),
(12, 'sdgdsfg'),
(13, 'asdasdaas'),
(14, 'lksdfélkd');

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `id` int(11) NOT NULL,
  `usr` varchar(255) COLLATE utf8_hungarian_ci NOT NULL,
  `psw` varchar(255) COLLATE utf8_hungarian_ci NOT NULL,
  `token` varchar(255) COLLATE utf8_hungarian_ci NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`id`, `usr`, `psw`, `token`) VALUES
(1, 'admin', '$2y$10$akuZm4yOOcOW61LfdUFp1./4nWgZNtCH0hPxV9121WZxAyUJuEfe6', '5e1c67a9c63427.30384605');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `author`
--
ALTER TABLE `author`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `books`
--
ALTER TABLE `books`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `genre`
--
ALTER TABLE `genre`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `publisher`
--
ALTER TABLE `publisher`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `author`
--
ALTER TABLE `author`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `books`
--
ALTER TABLE `books`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT for table `genre`
--
ALTER TABLE `genre`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT for table `publisher`
--
ALTER TABLE `publisher`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
