   M S B A M L   `   `   `  ����Q  MGenealogicalTreeCource, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:GenealogicalTreeCource.ViewModel GenealogicalTreeCource.ViewModel  5      0    *GenealogicalTreeCource.OperationWithPerson   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      @local.clr-namespace:GenealogicalTreeCource.ViewModel     5        N� 
Background
  ����� N� Title$ 	AddPerson��5	       � 	Resources 5
      ?   9GenealogicalTreeCource.ViewModel.GenderToBooleanConverter   GenderToBooleanConverterB   <GenealogicalTreeCource.ViewModel.DateOnlyToDateTimeConverter  DateOnlyConverterB   <GenealogicalTreeCource.ViewModel.PersonToCollectionConverter  PersonToCollectionConverterQ   KGenealogicalTreeCource.ViewModel.LastElementCollectionToCollectionConverter . *LastElementCollectionToCollectionConverterL   FGenealogicalTreeCource.ViewModel.CollectionToStringCollectionConverter ) %CollectionToStringCollectionConverter �� 
TargetType �� Property �� Value��%�  &	        &	      &	 &     &	 9     &	 L     '�� _     '�� �     '�� ;      5   
   6)     5   
   6,     5   
   6,     5   
   6;     5   
   66   �� 5   
   " ��6   .�5      ��  � ��$ 18(�6)   �� 5       � ��$ Center=�62   5      �� 5   
   " ��6   .�5      ��  � ��$ 18(�6)   �� 5       � ��$ 27��6'   5      �� 5   
   " ��6   .�5      -   5        5      5      .��5      �  � RowDefinitions 5    
   �� 5!      $
��1.5* �6   �� 5"       � ColumnDefinitions 5$   
   �� 5%      $
��0.7* �6   �� 5&      �� 5'      .+�5*   
   �� $#��/Image/PersonPhoto/absent.jpg��6   � 5+   
   $��10q�6   $��1��6    5,      �� 5-      �� 5.      �� 5/      �� 50      �� 51      �� 52      �� 53      �� 54      .+�57      �� $��Ім'я��6   $��0��6$   �� 58      $��1��6>   ��6   ��  �� Path$
 Name�	 �� Mode$	 TwoWay=�6>   �� 5:      $��Прізвище��6   $��2��6(   �� 5;      $��3��6A   ��6   �� $ Surname�$	 TwoWay=�6A   �� 5=      $��По батькові��6   $��4��6+   �� 5>      $��5��6D   ��6   �� $ 
Fathername�$	 TwoWay=�6D   �� 5@      $��Батько��6   $��6��6&   �� 5A      $��7��6r   ��6   �� $ Father�
 �� 	Converter#
 [ 6r   5C      � 5E   
   $��10q�6   $��2��6    5F      �� 5G      �� 5H      �� 5I      �� 5J      �� 5K      �� 5L      �� 5M      �� 5N      .+�5Q      �� $��
Стать��6   $��0��6%   �� 5R      $��1��6    �� SelectedItem 6$   �� *GenderPerson+$	 TwoWay=�.@�5S      �� $��Чоловіча��6    �� 
IsSelected 62   �� *GenderPerson+#
 [   �� ConverterParameter$
 Male���� 5T      $��Жіноча��6    60   �� *GenderPerson+#
 [  $ Female��5U      �� 5X      $#��Дата народження��6   $��2��6/   (  "System.Windows.Controls.DatePicker  5Y      $��3��6      SelectedDate 6   �� $ BirthdayDate�#
 [ $	 TwoWay=�6   �� 5[      $��Дата смерті��6   $��4��6+     5\      $��5��6|    6   �� $ 	DeathDate�#
 [ $	 TwoWay=�6|   �� 5^      $��Мати��6   $��6��6$   �� 5_      $��7��6r   ��6   �� $ Mother�#
 [ 6r   5`      � 5b   
   $��10q�6   $��1��6   $��1��6+    5c      �� 5d      �� 5e      �� 5f      �� 5g      .+�5j      �� $��Шлюб з��6   $��0��6&   �� 5k      $��1��6�   ��6   �� $ Wifes�#
 [ 6�   �� 5m      $#��Попередні шлюби��6   $��2��6/   �� 5n      $��3��6�   ��6   �� $ Wifes�$	 OneWay=�#
 [ 6�   5o      � 5q   
   $��10q�6   $��2��6   $��1��6+    5r      �� 5s      $
��0.3* �6    �� 5t      .+�5w      �� $��Діти��6   $��0��6$   �� 5x      $��1��6�   ��6   �� *
Children+$	 OneWay=�#
 [ 6�   5y      5z      5{      