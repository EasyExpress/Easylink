   M S B A M L   `   `   `  ����   EasylinkApp.WPF$    EasylinkApp.WPF.EmployeeWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     �Name$  Window1�� � Title$ Employee��$	��220��$	��600��.����  �� Orientation$ Vertical=�$��0,10,0,0q�.+�� $��5q� � 	Resources  �� 
TargetType �� Property �� Value��%�   '��       '�� )     '�� N     �� " ��.���  � ��$ 0,0,0,10q��� " ��.���  � ��$
 Bold"��� " ��.���  � ��$
 Bold"� � ColumnDefinitions �� $��* ��� $
��1.4* ��� $��20 ��� $��* ��� $
��1.4* � � RowDefinitions �� �� �� �� �� .+��� $��0��$��0��First Name:�� $��1��$��0������ *	FirstName+�� $��3��$��0��
Last Name:�� $��4��$��0������ *
LastName+�� $��0��$��1��Department:�� $��1��$��1��	 �� DisplayMemberPath$
	 Name��
 �� SelectedValuePath$
 Id������ *Departments+ �� SelectedValue �� *
Department+�� $��3��$��1��	Active:�� $��4��$��1�� d� 	IsChecked �� *Active+�� $��0��$��2��	Salary:�� $��1��$��2������ *Salary+�� $��3��$��2��Employed Since:(  "System.Windows.Controls.DatePicker  $��4��$��2��   SelectedDate �� *EmployedSince+�� $��0��$��3��Age:�� $��1��$��3������ *Age+�� $��3��$��3��	Weight:�� $��4��$��3������ *Weight+�� $��0��$��4��
Married:�� $��1��$��4�� �� *	Married+]� ��. .+��� $��Cancel��$��80��$��5q�$��Right=�����  �� Path$ RunCancelCommand�����  �� ElementName$ Window1���� $
��Save��$��80��$��5q�$��Right=����� $ RunSaveCommand����� $ Window1���� 
���  ��$	��350��$��5q�$
��Left=����� $ ValidationResult�