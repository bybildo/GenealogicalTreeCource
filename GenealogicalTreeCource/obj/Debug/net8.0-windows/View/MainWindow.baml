   M S B A M L   `   `   `  ����Q  MGenealogicalTreeCource, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:GenealogicalTreeCource.ClassGenealogicalTreeCource.Class  5   	   '    !GenealogicalTreeCource.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   6local*clr-namespace:GenealogicalTreeCource.Class  5   	   _ [MaterialDesignThemes.Wpf, Version=5.1.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962a:md1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � 
ResizeMode$  NoResize=�5
   	    � Title$+ %Генеалогічне Дерево��5   	   $	��670��5   	   $
��1000��6   
������� � WindowStyle$ SingleBorderWindow=�5   	    � Icon$ /Image/leaf_icon.ico��5   	    � 	Resources 5      -   'GenealogicalTreeCource.Class.PersonTree   MyPersonTree �� 
TargetType �� Property �� Value �� 
StartPoint	 �� EndPoint
 � Color � Offset3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   ShadowDepth   Opacity   Color �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value�� 5   
   %�  &	        '��        5      6   �� 5      " ��6   .�5      ��  � ��$ 80��6+   �� 5       � ��$ 30(�6-   �� 5       � ��$ Comic Sans MS)�6/   �� 5       � ��$	 300��6*   �� 5       � ��
 �����6/   �� 5       � ��$ 20q�6,   �� 5       � ��$ 0,0,0,20q�6+   �� 5       � �� 5      �� 5      $	 0,0&�6.   $		 1,0&�6?   .��5      � $
 #4CAF50��6+   $	 0.0Q�6;   � 5       $
 #124213��6+   $	 1.0Q�6;   5!      5#      �� 5$       �@J�Effect 5%        5&      $ 15Q�6+   $ 5Q�6;   $	 0.4Q�6K   $ Black��6Y   5(      �� 5)       � �� 5*      �� 5+      " ��6*   .R�5,      � .+�5-   "   �� #��z��6)   $ 40��6S   ._�5.   &   �� $��Center=�67   $��Center=�6T   5/   #   50      51      53       54      P� 55       �@J�IsMouseOver$
 True��65   . �56      ��  �@J�Opacity$	 0.8Q�64   57      59      5:       � DataContext 5=      �� 5>   
    �� ResourceKey$ MyPersonTree��6   .��5A      � ��5B   
   �� 5C       �� ImageSource$+ %/Image/MainWindow/tree_background.jpg��6    j� Stretch$ UniformToFill=�6M   .+�5G   
   �� 
���   � J� Opacity$	 0.5Q�6!   $��Stretch=�6/   $��Stretch=�6M   �� 5I   
   $��/Image/leaf.png��6   $��444,37,444,483q�6)   �� 5K   
   $��GENEALOGICAL TREE��6   $��40(�5L      $��	UltraBold"�6   
������$��Comic Sans MS)�5M      $��Center=�5N      $	��Top=�6)   $��	0,160,0,0q�5O      $
��Wrap=�6    �� TextAlignment$ Center=�5P       �� 
LineHeight$ 40��6#   $	��748��63    J� Effect 5Q        5R      $ 2Q�6#   $ 5Q�63   $ Black��6B   5T      �� 5V   
   $A��;Твоє власне генеалогічне дерево��6   $��36(�5W      
������$��Comic Sans MS)�5X      $��Center=�5Y      $	��Top=�61   $��	0,215,0,0q�5Z      $
��Wrap=�6'   $ Center=�5[      $ 40��6+   $	��748��6;    5\        5]      $ 2Q�6#   $ 5Q�63   $ Black��6B   5_      �� 5a   
   $	��Top=�6   $��Center=�6-   $��	0,303,0,0q�6J   .+�5c      �� $��ТВОЄ ДЕРЕВО��6   ��6,   �� *	GraphPage+�� 5d      $��ДОДАТИ��6   ��6&   �� *AddPersonPage+5e      �� 5i   
   -   5        $��Right=�5i      $	��Top=�6-   $��
0,527,54,0q�6E   $	��100��5j   	   $��70��6   
������ ._�5l      �� -   5         ��Name$ AdministratorButton��5l      $��/Image/admin.png��61   $��82��6K   $��70��6V   $��Center=�5m      $��Center=�6)   $y�Hidden=�6E    5o        5p      $ 10Q�6'   $ 3Q�67   $	 0.5Q�6G   $ Black��6U   ��5r      �� 5s      " ��6   .�5t      ��  �@J�Opacity$	 0.5Q�64    5u      P� 5v       �@J�IsMouseOver$
 True��6=   . �5w   "   ��  �@J�Opacity$ 1Q�6<   5x      5z      *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5|      +  %Microsoft.Xaml.Behaviors.EventTrigger  5}         	EventName$ MouseLeftButtonDown��6%   *  $Microsoft.Xaml.Behaviors.TriggerBase    Actions.  5~      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction  !   Command! 60   �� *AdministrationPage+5      5�      5�      �� 5�   
   $��0q�6   ._�5�   
    � -   5        "  �Name$" 	SetWindow��5�      #  � NavigationUIVisibility$# Hidden=�6#   5�      5�      5�      