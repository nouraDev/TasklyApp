import { useState, useEffect } from "react";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "../ui/dialog";
import { Input } from "../ui/input";
import { Button } from "../ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../ui/select";
import { Label } from "../ui/label";

type CategoryFormDialogProps = {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSubmit: (data: { name: string; color: string }) => Promise<void>;
  initialData?: { name: string; color: string };
  mode?: "create" | "edit";
};

const predefinedColors = [
  { name: "Charcoal", value: "#374151" },
  { name: "Red", value: "#ef4444" },
  { name: "Blue", value: "#3b82f6" },
  { name: "Green", value: "#10b981" },
  { name: "Yellow", value: "#f59e0b" },
  { name: "Purple", value: "#8b5cf6" },
];

export function CategoryFormDialog({
  open,
  onOpenChange,
  onSubmit,
  initialData,
  mode = "create",
}: Readonly<CategoryFormDialogProps>) {
  const [name, setName] = useState("");
  const [color, setColor] = useState(predefinedColors[0].value);
  const [loading, setLoading] = useState(false);
  const [dynamicColors, setDynamicColors] = useState(predefinedColors);

  useEffect(() => {
    if (initialData) {
      setName(initialData.name);
      setColor(initialData.color);
      if (!dynamicColors.some((c) => c.value === initialData.color)) {
        setDynamicColors((prev) => [
          ...prev,
          { name: "Custom Color", value: initialData.color },
        ]);
      }
    } else {
      setName("");
      setColor(predefinedColors[0].value);
    }
  }, [initialData, open]);

  const handleSubmit = async () => {
    if (!name.trim()) return;
    setLoading(true);
    try {
      await onSubmit({ name, color });
      onOpenChange(false);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-lg bg-white dark:bg-gray-900 rounded-xl shadow-lg p-6">
        <DialogHeader>
          <DialogTitle className="text-lg font-semibold">
            {mode === "create" ? "Add section" : "Modify section"}
          </DialogTitle>
        </DialogHeader>

        <div className="space-y-6 max-h-[65vh] overflow-y-auto pr-2">
          {/* Name */}
          <div>
            <Label htmlFor="name">Name</Label>
            <Input
              placeholder="Section name"
              maxLength={120}
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
            <p className="text-xs text-gray-400 text-right">{name.length}/120</p>
          </div>

          {/* Color */}
          <div>
            <Label className="text-sm font-medium">Color</Label>
            <Select value={color} onValueChange={setColor}>
              <SelectTrigger className="bg-gray-100 dark:bg-gray-800">
                <SelectValue />
              </SelectTrigger>
              <SelectContent className="bg-white dark:bg-gray-800 z-50">
                {dynamicColors.map((c) => (
                  <SelectItem key={c.value} value={c.value}>
                    <div className="flex items-center gap-2">
                      <span
                        className="w-4 h-4 rounded-full"
                        style={{ backgroundColor: c.value }}
                      />
                      {c.name}
                    </div>
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>
        </div>

        <DialogFooter className="pt-4">
          <Button variant="ghost" onClick={() => onOpenChange(false)}>
            Cancel
          </Button>
          <Button onClick={handleSubmit} disabled={loading}>
            {mode === "create" ? "Add" : "Save Changes"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
